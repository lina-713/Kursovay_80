begin
CREATE OR REPLACE PROCEDURE public.add_new_athlet(
	IN newfirstname text,
	IN newname text,
	IN newheight integer,
	IN newweight integer,
	IN newid_team integer)
LANGUAGE 'sql'
AS $BODY$
insert into athletes(firstname, name, height, weight, id_team) 
	values (newfirstname, newname, newheight, newweight, newid_team);
$BODY$;
ALTER PROCEDURE public.add_new_athlet(text, text, integer, integer, integer)
    OWNER TO postgres;


CREATE OR REPLACE PROCEDURE public.add_new_match(
	IN new_idteam1 integer,
	IN new_idteam2 integer,
	IN new_date timestamp without time zone,
	IN new_score1 integer,
	IN new_score2 integer,
	IN new_idstadion integer)
LANGUAGE 'sql'
AS $BODY$
insert into match_schedule(id_team1, id_team2, date_of_match, team1_score, team2_score, idstadion)
values(new_idteam1, new_idteam2, new_date, new_score1, new_score2, new_idstadion);
$BODY$;
ALTER PROCEDURE public.add_new_match(integer, integer, timestamp without time zone, integer, integer, integer)
    OWNER TO postgres;


CREATE OR REPLACE PROCEDURE public.add_new_stadion(
	IN newcity text,
	IN newname text,
	IN newcapacity integer)
LANGUAGE 'sql'
AS $BODY$
INSERT INTO info_about_location (city, name, capacity)
VALUES (newcity, newname, newcapacity);

-- Если все операции прошли успешно, коммитим транзакцию
--COMMIT;

-- Если произошла ошибка, отменяем транзакцию
--ROLLBACK;
$BODY$;
ALTER PROCEDURE public.add_new_stadion(text, text, integer)
    OWNER TO postgres;

COMMIT;
end;