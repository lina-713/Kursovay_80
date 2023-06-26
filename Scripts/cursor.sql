CREATE OR REPLACE PROCEDURE public.curs_proc(
	)
LANGUAGE 'plpgsql'
AS $BODY$
declare 
curs cursor for Select * from view_teams;
max_res int;
rec RECORD;
BEGIN
open curs;
loop
fetch curs into rec;
exit when not found;
max_res := (select max(results_score) from view_teams);
if rec.results_score = max_res then 
	update view_teams
	set contender = 'Претендент на победу' where view_teams.idteam = rec.idteam;
else if rec.results_score < max_res then 
	update view_teams
	set contender = 'Не претендент на победу' where view_teams.idteam = rec.idteam;
end if;
end if;
end loop;
close curs;
end;
$BODY$;
ALTER PROCEDURE public.curs_proc()
    OWNER TO postgres;