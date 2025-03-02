CREATE SEQUENCE IF NOT EXISTS messages_messageid_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

CREATE SEQUENCE IF NOT EXISTS messages_sequencenumber_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

CREATE TABLE IF NOT EXISTS messages
(
    messageid integer NOT NULL DEFAULT nextval('messages_messageid_seq'::regclass),
    text character varying(128) COLLATE pg_catalog."default" NOT NULL,
    "timestamp" timestamp without time zone NOT NULL DEFAULT now(),
    sequencenumber integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    CONSTRAINT messages_pkey PRIMARY KEY (messageid)
    )

    TABLESPACE pg_default;

CREATE FUNCTION getall(
	)
    RETURNS TABLE(messageid integer, text character varying, "timestamp" timestamp without time zone, sequencenumber integer) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN
RETURN QUERY
SELECT * FROM messages;
END;
$BODY$;

CREATE PROCEDURE insertmessage(
	IN p_text character varying,
	IN p_timestamp timestamp without time zone)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
BEGIN
INSERT INTO Messages(text, timestamp)
VALUES (p_Text, p_TimeStamp);
EXCEPTION WHEN OTHERS THEN
        RAISE NOTICE 'Error: %', SQLERRM;
        RAISE;
END;
END;
$BODY$;
