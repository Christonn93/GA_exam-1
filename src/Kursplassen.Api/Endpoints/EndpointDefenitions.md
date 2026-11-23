# Obligatoriske endepunkter – oversikt

Følger tabellen i `docs/Assignment.md` (Oppgave 1 – Implementering av REST API).

| Metode | Rute                                    | Tilgang           | Beskrivelse                                            |
|--------|-----------------------------------------|-------------------|--------------------------------------------------------|
| POST   | `/api/auth/register`                    | Ingen             | Oppretter deltakerkonto; rollen settes av systemet.    |
| POST   | `/api/auth/login`                       | Ingen             | Validerer innlogging og returnerer JWT.                |
| GET    | `/api/courses`                          | Ingen             | Lister publiserte, fremtidige kurs.                    |
| GET    | `/api/courses/{courseId}`               | Ingen             | Viser kursdetaljer, antall påmeldte og ledige plasser. |
| POST   | `/api/courses`                          | Instructor        | Oppretter kurs; innlogget instruktør blir eier.        |
| PUT    | `/api/courses/{courseId}`               | CourseOwner       | Oppdaterer eget kurs.                                  |
| DELETE | `/api/courses/{courseId}`               | CourseOwner       | Slater kurs uten aktive påmeldinger.                   |
| POST   | `/api/courses/{courseId}/registrations` | Participant       | Melder innlogget deltaker på kurset.                   |
| GET    | `/api/registrations/mine`               | Participant       | Viser egne påmeldinger med kursinformasjon.            |
| DELETE | `/api/registrations/{registrationId}`   | RegistrationOwner | Avbestiller egen påmelding og frigjør plassen.         |
| GET    | `/health`                               | Ingen             | Rapporterer helsetilstand.                             |

## Tilgangsbetegnelser

- **Ingen** – offentlige endepunkter (ingen autentisering).
- **Participant** – innloggeds bruker med `Participant`-rolle; kan se og avbestille egne påmeldinger, melde seg på kurs.
- **Instructor** – innloggeds bruker med `Instructor`-rolle; kan opprette og administrere kurs.
- **CourseOwner** – tilgangsbetegnelse for en `Instructor` som eier det aktuelle kurset; ikke en egen konto- eller JWT-rolle.
- **RegistrationOwner** – tilgangsbetegnelse for en `Participant` som eier den aktuelle påmeldingen; ikke en egen konto- eller JWT-rolle.

## Statuskoder og anbefalt praksis

- `200 OK` – succes.
- `201 Created` – risikerte ressurser (f.eks. opprettet kurs / påmelding).
- `204 No Content` – sletting (f.eks. avbestilling).
- `400 Bad Request` – validering av inndata.
- `401 Unauthorized` – manglende/ugyldig autentisering.
- `403 Forbidden` – autentisert, men ikke riktig eierskap/rolle.
- `404 Not Found` – ressursen finnes ikke.
- `409 Conflict` – fordi (f.eks. fullt kurs, dobbel aktiv påmelding).
