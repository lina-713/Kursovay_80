CREATE OR REPLACE FUNCTION public.count_matches_per_stadium(
	id_stadion integer)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
matches_count INTEGER;
BEGIN
SELECT COUNT(*) INTO matches_count FROM match_schedule WHERE id_stadion = $1;
RETURN matches_count;
END;
$BODY$;

ALTER FUNCTION public.count_matches_per_stadium(integer)
    OWNER TO postgres;