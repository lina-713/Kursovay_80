BEGIN
CREATE OR REPLACE FUNCTION public.audit_func()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF
AS $BODY$
DECLARE
    v_old_data TEXT;
    v_new_data TEXT;
BEGIN
    if (TG_OP = 'UPDATE') then
        v_old_data := ROW(OLD.*);
        v_new_data := ROW(NEW.*);
        insert into audit (table_name, user_name, action, original_data, new_data, query) 
        values (TG_TABLE_NAME::TEXT, session_user::TEXT, substring(TG_OP,1,1), v_old_data, v_new_data, current_query());
        RETURN NEW;
    elsif (TG_OP = 'DELETE') then
        v_old_data := ROW(OLD.*);
        insert into audit (table_name, user_name, action, original_data, query)
        values (TG_TABLE_NAME::TEXT, session_user::TEXT, substring(TG_OP,1,1), v_old_data, current_query());
        RETURN OLD;
    elsif (TG_OP = 'INSERT') then
        v_new_data := ROW(NEW.*);
        insert into audit (table_name, user_name, action, new_data, query)
        values (TG_TABLE_NAME::TEXT, session_user::TEXT,substring(TG_OP,1,1), v_new_data, current_query());
        RETURN NEW;
    else
        RAISE WARNING '[AUDIT.IF_MODIFIED_FUNC] - Other action occurred: %, at %',TG_OP,now();
        RETURN NULL;
    end if;
END;
$BODY$;

ALTER FUNCTION public.audit_func()
    OWNER TO postgres;

CREATE TRIGGER athletes_audit
AFTER INSERT OR UPDATE OR DELETE ON athletes
FOR EACH ROW EXECUTE PROCEDURE audit_func();

CREATE TRIGGER teams_audit
AFTER INSERT OR UPDATE OR DELETE ON teams
FOR EACH ROW EXECUTE PROCEDURE audit_func();

CREATE TRIGGER info_about_location_audit
AFTER INSERT OR UPDATE OR DELETE ON info_about_location
FOR EACH ROW EXECUTE PROCEDURE audit_func();

COMMIT;
END;