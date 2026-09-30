PAUSE "Eliminant registres..."

DELETE FROM parada;
DELETE FROM trajecte;
DELETE FROM estacio;
DELETE FROM usuari;
DELETE FROM mitja_transport;
DELETE FROM ciutat;
COMMIT;

PAUSE "Inserint ciutats..."

INSERT INTO ciutat (nom) VALUES ('Barcelona');
INSERT INTO ciutat (nom) VALUES ('Girona');
INSERT INTO ciutat (nom) VALUES ('Tarragona');
INSERT INTO ciutat (nom) VALUES ('Lleida');
INSERT INTO ciutat (nom) VALUES ('Igualada');
INSERT INTO ciutat (nom) VALUES ('Manresa');
INSERT INTO ciutat (nom) VALUES ('Reus');
INSERT INTO ciutat (nom) VALUES ('Sabadell');
INSERT INTO ciutat (nom) VALUES ('Terrassa');
INSERT INTO ciutat (nom) VALUES ('Vic');

PAUSE "Inserint estacions..."

INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='BARCELONA'), 'Barcelona Sants',            41.37946, 2.14003);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='BARCELONA'), 'Barcelona Estació de França', 41.38468, 2.18573);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='BARCELONA'), 'Barcelona Plaça Catalunya',   41.38696, 2.17000);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='BARCELONA'), 'Barcelona Passeig de Gràcia', 41.39190, 2.16450);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='BARCELONA'), 'Barcelona Plaça Espanya',    41.37460, 2.14900);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='GIRONA'),    'Girona',                      41.9794, 2.8183);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='TARRAGONA'), 'Tarragona',                   41.11127, 1.25350);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='TARRAGONA'), 'Camp de Tarragona',           41.19218, 1.27406);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='LLEIDA'),    'Lleida Pirineus',             41.6208, 0.6328);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='REUS'),      'Reus',                        41.1605, 1.10015);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='IGUALADA'),  'Igualada',                    41.578, 1.62998);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='MANRESA'),   'Manresa',                     41.7204, 1.82644);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='SABADELL'),  'Sabadell Centre',             41.5464, 2.11562);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='SABADELL'),  'Sabadell Nord',               41.5620, 2.0962);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='TERRASSA'),  'Terrassa',                    41.57093, 2.01576);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='TERRASSA'),  'Terrassa Rambla',             41.56055, 2.0075);
INSERT INTO estacio (IDCiutat, nom, latitud, longitud) VALUES ((SELECT ID FROM ciutat WHERE UPPER(nom)='VIC'),       'Vic',                         41.93102, 2.24886);

PAUSE "Inserint mitjans de transport..."

INSERT INTO mitja_transport (nom) VALUES ('Rodalies');
INSERT INTO mitja_transport (nom) VALUES ('FGC');
INSERT INTO mitja_transport (nom) VALUES ('Alta Velocitat (AVE)');
INSERT INTO mitja_transport (nom) VALUES ('Avant');
INSERT INTO mitja_transport (nom) VALUES ('Metro');
INSERT INTO mitja_transport (nom) VALUES ('Tramvia');
INSERT INTO mitja_transport (nom) VALUES ('Autobús urbà');
INSERT INTO mitja_transport (nom) VALUES ('Autobús interurbà');

PAUSE "Inserint usuaris..."

INSERT INTO usuari (email, nom, password, tipus) VALUES ('admin1@example.com',     'Marta Puig',    'Admin1234!',   'A');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('admin2@example.com',     'Jordi Serra',   'Admin5678!',   'A');

INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor1@example.com',    'Laia Ferrer',   'Editor1Pass!', 'E');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor2@example.com',    'Pau Vila',      'Editor2Pass!', 'E');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor3@example.com',    'Núria Soler',   'Editor3Pass!', 'E');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor4@example.com',    'Oriol Camps',   'Editor4Pass!', 'E');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor5@example.com',    'Clàudia Roca',  'Editor5Pass!', 'E');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('editor6@example.com',    'Arnau Mas',     'Editor6Pass!', 'E');

INSERT INTO usuari (email, nom, password, tipus) VALUES ('bloquejat1@example.com', 'Sergi Bosch',   'Bloc1Pass!',   'B');
INSERT INTO usuari (email, nom, password, tipus) VALUES ('bloquejat2@example.com', 'Anna Molina',   'Bloc2Pass!',   'B');

PAUSE "Inserint trajectes..."

INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('R11 Barcelona Sants - Girona', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Rodalies')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR1@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('R14 Barcelona França - Lleida (per Reus)', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Rodalies')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR2@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('R4 Manresa - Barcelona Sants', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Rodalies')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR3@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('FGC S1 Plaça Catalunya - Terrassa Rambla', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('FGC')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR4@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('R4 Terrassa - Sabadell', 0,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Rodalies')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR5@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('R3 Barcelona Plaça Catalunya - Vic', 0,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Rodalies')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR6@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('FGC R6 Plaça Espanya - Igualada', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('FGC')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR1@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('AVE Barcelona Sants - Lleida Pirineus', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Alta Velocitat (AVE)')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR2@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('Avant Barcelona Sants - Girona', 0,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Avant')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR3@EXAMPLE.COM'));
INSERT INTO trajecte (nom, validat, mitjaTransport, usuariCreador) VALUES ('Metro L3 Sants - Passeig de Gràcia', 1,
    (SELECT ID FROM mitja_transport WHERE UPPER(nom)=UPPER('Metro')),
    (SELECT ID FROM usuari WHERE UPPER(email)='EDITOR4@EXAMPLE.COM'));

PAUSE "Inserint parades..."

-- R11 Barcelona Sants - Girona
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '07:05', e.ID FROM trajecte t, estacio e WHERE t.nom='R11 Barcelona Sants - Girona' AND e.nom='Barcelona Sants';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '08:40', e.ID FROM trajecte t, estacio e WHERE t.nom='R11 Barcelona Sants - Girona' AND e.nom='Girona';

-- R14 Barcelona França - Lleida (per Reus)
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '06:30', e.ID FROM trajecte t, estacio e WHERE t.nom='R14 Barcelona França - Lleida (per Reus)' AND e.nom='Barcelona Estació de França';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '07:50', e.ID FROM trajecte t, estacio e WHERE t.nom='R14 Barcelona França - Lleida (per Reus)' AND e.nom='Tarragona';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 3, t.ID, '08:10', e.ID FROM trajecte t, estacio e WHERE t.nom='R14 Barcelona França - Lleida (per Reus)' AND e.nom='Reus';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 4, t.ID, '09:40', e.ID FROM trajecte t, estacio e WHERE t.nom='R14 Barcelona França - Lleida (per Reus)' AND e.nom='Lleida Pirineus';

-- R4 Manresa - Barcelona Sants
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '05:54', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Manresa - Barcelona Sants' AND e.nom='Manresa';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '06:38', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Manresa - Barcelona Sants' AND e.nom='Terrassa';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 3, t.ID, '06:55', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Manresa - Barcelona Sants' AND e.nom='Sabadell Centre';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 4, t.ID, '07:21', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Manresa - Barcelona Sants' AND e.nom='Barcelona Sants';

-- FGC S1 Plaça Catalunya - Terrassa Rambla
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '08:00', e.ID FROM trajecte t, estacio e WHERE t.nom='FGC S1 Plaça Catalunya - Terrassa Rambla' AND e.nom='Barcelona Plaça Catalunya';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '08:33', e.ID FROM trajecte t, estacio e WHERE t.nom='FGC S1 Plaça Catalunya - Terrassa Rambla' AND e.nom='Terrassa Rambla';

-- R4 Terrassa - Sabadell
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '09:15', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Terrassa - Sabadell' AND e.nom='Terrassa';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '09:25', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Terrassa - Sabadell' AND e.nom='Sabadell Nord';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 3, t.ID, '09:30', e.ID FROM trajecte t, estacio e WHERE t.nom='R4 Terrassa - Sabadell' AND e.nom='Sabadell Centre';

-- R3 Barcelona Plaça Catalunya - Vic
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '07:00', e.ID FROM trajecte t, estacio e WHERE t.nom='R3 Barcelona Plaça Catalunya - Vic' AND e.nom='Barcelona Plaça Catalunya';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '08:15', e.ID FROM trajecte t, estacio e WHERE t.nom='R3 Barcelona Plaça Catalunya - Vic' AND e.nom='Vic';

-- FGC R6 Plaça Espanya - Igualada
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '08:00', e.ID FROM trajecte t, estacio e WHERE t.nom='FGC R6 Plaça Espanya - Igualada' AND e.nom='Barcelona Plaça Espanya';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '09:05', e.ID FROM trajecte t, estacio e WHERE t.nom='FGC R6 Plaça Espanya - Igualada' AND e.nom='Igualada';

-- AVE Barcelona Sants - Lleida Pirineus (via Camp de Tarragona)
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '07:00', e.ID FROM trajecte t, estacio e WHERE t.nom='AVE Barcelona Sants - Lleida Pirineus' AND e.nom='Barcelona Sants';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '07:32', e.ID FROM trajecte t, estacio e WHERE t.nom='AVE Barcelona Sants - Lleida Pirineus' AND e.nom='Camp de Tarragona';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 3, t.ID, '07:58', e.ID FROM trajecte t, estacio e WHERE t.nom='AVE Barcelona Sants - Lleida Pirineus' AND e.nom='Lleida Pirineus';

-- Avant Barcelona Sants - Girona
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '08:30', e.ID FROM trajecte t, estacio e WHERE t.nom='Avant Barcelona Sants - Girona' AND e.nom='Barcelona Sants';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '09:08', e.ID FROM trajecte t, estacio e WHERE t.nom='Avant Barcelona Sants - Girona' AND e.nom='Girona';

-- Metro L3 Sants - Passeig de Gràcia
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 1, t.ID, '09:00', e.ID FROM trajecte t, estacio e WHERE t.nom='Metro L3 Sants - Passeig de Gràcia' AND e.nom='Barcelona Sants';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 2, t.ID, '09:13', e.ID FROM trajecte t, estacio e WHERE t.nom='Metro L3 Sants - Passeig de Gràcia' AND e.nom='Barcelona Plaça Catalunya';
INSERT INTO parada (ordre, idTrajecte, horaPas, idEstacio) SELECT 3, t.ID, '09:15', e.ID FROM trajecte t, estacio e WHERE t.nom='Metro L3 Sants - Passeig de Gràcia' AND e.nom='Barcelona Passeig de Gràcia';

COMMIT;