CREATE OR REPLACE PROCEDURE public.update_athlet(
	IN new_firstname text,
	IN new_name text,
	IN new_height integer,
	IN new_weight integer,
	IN new_idteam integer,
	IN new_id integer)
LANGUAGE 'sql'
AS $BODY$
UPDATE athletes 
	SET	firstname = new_firstname,
	name = new_name,
	height = new_height,
	weight = new_weight,
	id_team = new_idteam
	where id = new_id
$BODY$;
ALTER PROCEDURE public.update_athlet(text, text, integer, integer, integer, integer)
    OWNER TO postgres;
CREATE OR REPLACE PROCEDURE public.update_match(
	IN new_idteam1 integer,
	IN new_idteam2 integer,
	IN new_date timestamp without time zone,
	IN new_score1 integer,
	IN new_score2 integer,
	IN new_idstadion integer,
	IN _id integer)
LANGUAGE 'sql'
AS $BODY$
update match_schedule 
	set 
	id_team1 = new_idteam1, 
	id_team2 = new_idteam2, 
	date_of_match = new_date, 
	team1_score = new_score1, 
	team2_score = new_score2, 
	idstadion = new_idstadion
	where id = _id
$BODY$;
ALTER PROCEDURE public.update_match(integer, integer, timestamp without time zone, integer, integer, integer, integer)
    OWNER TO postgres;
CREATE OR REPLACE PROCEDURE public.update_stadion(
	IN new_city text,
	IN new_capacity integer,
	IN new_name text,
	IN new_id integer)
LANGUAGE 'sql'
AS $BODY$
update info_about_location
set city = new_city, 
	capacity = new_capacity, 
	name = new_name
	where id_stadion = new_id
$BODY$;
ALTER PROCEDURE public.update_stadion(text, integer, text, integer)
    OWNER TO postgres;


CREATE OR REPLACE PROCEDURE public.update_teams(
	IN up_name_team text,
	IN up_date timestamp without time zone,
	IN up_coach_lastname text,
	IN up_coach_name text,
	IN up_id integer)
LANGUAGE 'sql'
AS $BODY$
update teams 
set name_team = up_name_team, 
	date_of_foundation = up_date,
	coach_lastname = up_coach_lastname,
	coach_name = up_coach_name
	where idteam = up_id
$BODY$;
ALTER PROCEDURE public.update_teams(text, timestamp without time zone, text, text, integer)
    OWNER TO postgres;