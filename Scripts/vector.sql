CREATE OR REPLACE FUNCTION public.fill_stadions(
	col_1 text,
	col_2 text,
	col_3 text,
	col_4 text)
    RETURNS TABLE(id_stadion integer, citu text, capacity integer, name text, count_matches integer) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
begin
	
	RETURN QUERY EXECUTE format('select DISTINCT %I, %I, %I, %I, (select * from count_matches_per_stadium(match_schedule.idstadion)) AS matches_played  
								from info_about_location
								left JOIN match_schedule ON info_about_location.id_stadion = match_schedule.idstadion;', col_1, col_2, col_3, col_4);
end;
$BODY$;

ALTER FUNCTION public.fill_stadions(text, text, text, text)
    OWNER TO postgres;
