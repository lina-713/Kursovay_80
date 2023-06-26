BEGIN
CREATE OR REPLACE PROCEDURE public.delete_athlet(
	IN new_id integer)
LANGUAGE 'sql'
AS $BODY$
delete from athletes where athletes.id = new_id
$BODY$;
ALTER PROCEDURE public.delete_athlet(integer)
    OWNER TO postgres;

CREATE OR REPLACE PROCEDURE public.delete_match(
	IN delete_id integer)
LANGUAGE 'sql'
AS $BODY$
delete from match_schedule where match_schedule.id = delete_id
$BODY$;
ALTER PROCEDURE public.delete_match(integer)
    OWNER TO postgres;

CREATE OR REPLACE PROCEDURE public.delete_stadion(
	IN delete_id integer)
LANGUAGE 'sql'
AS $BODY$
delete from info_about_location where info_about_location.id_stadion = delete_id
$BODY$;
ALTER PROCEDURE public.delete_stadion(integer)
    OWNER TO postgres;


CREATE OR REPLACE PROCEDURE public.delete_teams(
	IN delete_id integer)
LANGUAGE 'sql'
AS $BODY$
delete from teams where teams.idteam = delete_id
$BODY$;
ALTER PROCEDURE public.delete_teams(integer)
    OWNER TO postgres;
COMMINT;
END;
