-- La primera vegada peta per el drop, però es pot seguir executant sense problema.

DROP TABLE ciutat CASCADE CONSTRAINTS;

CREATE TABLE ciutat (
	ID	NUMBER GENERATED ALWAYS AS IDENTITY,
	nom	VARCHAR2(100 CHAR) CONSTRAINT nn_ciutat_nom NOT NULL,

	CONSTRAINT pk_ciutat PRIMARY KEY(ID)
);

PAUSE "Taula ciutat creada..."

DROP TABLE estacio CASCADE CONSTRAINTS;

CREATE TABLE estacio (
	ID		    NUMBER GENERATED ALWAYS AS IDENTITY,
	IDCiutat	NUMBER CONSTRAINT nn_estacio_ciutat NOT NULL,
	nom         VARCHAR2(100 CHAR) CONSTRAINT nn_estacio_nom NOT NULL,
	latitud		NUMBER(8,5) CONSTRAINT nn_estacio_latitud NOT NULL,
	longitud	NUMBER(8,5) CONSTRAINT nn_estacio_longitud NOT NULL,

	CONSTRAINT pk_estacio PRIMARY KEY(ID),
	CONSTRAINT fk_estacio_ciutat FOREIGN KEY(IDCiutat) REFERENCES ciutat(ID)
);

PAUSE "Taula estació creada..."

DROP TABLE mitja_transport CASCADE CONSTRAINTS;

CREATE TABLE mitja_transport (
	ID	NUMBER GENERATED ALWAYS AS IDENTITY,
	nom	VARCHAR2(100 CHAR) CONSTRAINT nn_transport_nom NOT NULL,

	CONSTRAINT pk_transport PRIMARY KEY(ID)
);

PAUSE "Taula mitja_transport creada..."

DROP TABLE usuari CASCADE CONSTRAINTS;

CREATE TABLE usuari (
	ID		        NUMBER GENERATED ALWAYS AS IDENTITY,
	email		    VARCHAR2(100 CHAR) CONSTRAINT nn_usuari_email NOT NULL,
	nom		        VARCHAR2(100 CHAR),
	password	    VARCHAR2(100 CHAR) CONSTRAINT nn_usuari_password NOT NULL,
	tipus		    CHAR(1) CONSTRAINT nn_usuari_tipus NOT NULL,
	dataRegistre	DATE DEFAULT SYSDATE CONSTRAINT nn_usuari_data NOT NULL,

	CONSTRAINT pk_usuari PRIMARY KEY(ID),
	CONSTRAINT ck_usuari_tipus CHECK (tipus IN ('A', 'E', 'B'))
);

PAUSE "Taula usuari creada..."

DROP TABLE trajecte CASCADE CONSTRAINTS;

CREATE TABLE trajecte (
	ID		        NUMBER GENERATED ALWAYS AS IDENTITY,
	nom		        VARCHAR2(100 CHAR) CONSTRAINT nn_trajecte_nom NOT NULL,
	dataCreacio	    TIMESTAMP DEFAULT SYSDATE CONSTRAINT nn_trajecte_creacio NOT NULL,
	validat		    NUMBER(1) DEFAULT 0 CONSTRAINT nn_trajecte_validat NOT NULL,
	mitjaTransport	NUMBER CONSTRAINT nn_trajecte_transport NOT NULL,
	usuariCreador	NUMBER CONSTRAINT nn_trajecte_usuari NOT NULL,

	CONSTRAINT pk_trajecte PRIMARY KEY (ID),
	CONSTRAINT fk_trajecte_transport FOREIGN KEY (mitjaTransport) REFERENCES mitja_transport(ID),
	CONSTRAINT fk_trajecte_usuari FOREIGN KEY (usuariCreador) REFERENCES usuari(ID),
    CONSTRAINT ck_trajecte_validat CHECK (validat IN (0,1))
);

PAUSE "Taula trajecte creada..."

DROP TABLE parada CASCADE CONSTRAINTS;

CREATE TABLE parada (
	ordre		NUMBER,
	idTrajecte	NUMBER CONSTRAINT nn_parada_trajecte NOT NULL,
	horaPas	    VARCHAR2(5 CHAR) CONSTRAINT nn_parada_horaPas NOT NULL,
	idEstacio	NUMBER CONSTRAINT nn_parada_estacio NOT NULL,

	CONSTRAINT pk_parada PRIMARY KEY(ordre, idTrajecte),
	CONSTRAINT fk_parada_trajecte FOREIGN KEY(idTrajecte) REFERENCES trajecte(ID),
	CONSTRAINT fk_parada_estacio FOREIGN KEY(idEstacio) REFERENCES estacio(ID),
	CONSTRAINT ck_parada_horapas CHECK (REGEXP_LIKE(horaPas, '^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$'))
);

PAUSE "Taula parada creada..."
PAUSE "Procedint a crear els índexs únics (case insensitive)..."

CREATE UNIQUE INDEX idx_ciutat_nom ON ciutat (UPPER(nom));

CREATE UNIQUE INDEX idx_estacio_nom ON estacio (UPPER(IDCiutat||'.'||nom));

CREATE UNIQUE INDEX idx_transport_nom ON mitja_transport (UPPER(nom));

CREATE UNIQUE INDEX idx_usuari_email ON usuari (UPPER(email));

CREATE UNIQUE INDEX idx_trajecte_nom ON trajecte (UPPER(nom));

PAUSE "Índexs únics creats..."
PAUSE "Procedint a crear triggers..."

CREATE OR REPLACE TRIGGER trg_trajecte_data
BEFORE INSERT OR UPDATE ON trajecte 
FOR EACH ROW
BEGIN
    if INSERTING then
        :new.dataCreacio := SYSTIMESTAMP;
    elsif UPDATING then
        :new.dataCreacio := :old.dataCreacio;
    end if;
END;
/

CREATE OR REPLACE TRIGGER trg_usuari_data
BEFORE INSERT OR UPDATE ON usuari
FOR EACH ROW
BEGIN
    if INSERTING then
        :new.dataRegistre := SYSDATE;
    elsif UPDATING then
        :new.dataRegistre := :old.dataRegistre;
    end if;
END;
/

PAUSE "Triggers creats..."