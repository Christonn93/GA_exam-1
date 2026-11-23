# Eksamen – Emne 3 Avansert backend programmering

Du skal utvikle, sikre, teste og dokumentere et REST API i ASP.NET Core. Eksamen er individuell. Du skal kunne forklare og begrunne løsningen du leverer.

| Felt | Innhold |
|---|---|
| Emne | Emne 3 – Avansert backend-programmering |
| Modul | Modul 3 – Avansert programmering |
| Vurderingsform | Individuell hjemmeeksamen, bestått/ikke bestått |
| Utlevering | 20. november 2026 |
| Innlevering | 11. desember 2026 |
| Hjelpemidler | Alle, med krav til dokumentasjon av KI-bruk |
| Innlevering | Én ZIP-fil med løsning, dokumentasjon, tester, KI-dokumentasjon og video eller videolenke |

## Bruk av KI (kunstig intelligens)

Bruk av kunstig intelligens (KI) er tillatt i arbeidet med innleveringen. KI kan brukes som støtteverktøy, for eksempel til idéutvikling, forklaringer, feilsøking, strukturering, kodeforslag og språklig forbedring.

Du er selv ansvarlig for innholdet du leverer, og skal kunne forstå, forklare og begrunne løsningen din.

### Refleksjon over bruk av KI

Dersom du har brukt KI i arbeidet, skal du kort beskrive og reflektere over hvordan KI har påvirket besvarelsen. Du trenger ikke å dokumentere eller levere alle prompter og svar.

Refleksjonen skal kort beskrive:

- Hvordan KI ble brukt i arbeidet.
- Hva KI bidro med, for eksempel idéer, kode, forklaringer, feilsøking eller forbedringer.
- Hvordan KI påvirket den endelige løsningen.
- Hva du selv har vurdert, endret eller videreutviklet fra forslagene du fikk.
- Eventuelle feil, begrensninger eller utfordringer du oppdaget ved bruk av KI.

Refleksjonen skal være en del av innleveringen, enten i `README.md` eller i en egen fil.

Dersom du ikke har brukt KI, holder det å opplyse om dette.

### Viktig

- KI er et hjelpemiddel og erstatter ikke kandidatens egen forståelse. Du skal kunne gjøre rede for hele besvarelsen, inkludert kode, valg av løsning og sentrale faglige vurderinger.
- Ved tvil om kandidatens selvstendige forståelse av besvarelsen kan kandidaten bli bedt om å forklare eller demonstrere løsningen muntlig.
- Bruk av KI skal følge skolens gjeldende retningslinjer for KI, kildebruk og akademisk integritet.

## Scenario: Kursplassen

En lokal kursarrangør ønsker et API for publisering av kurs og påmelding av deltakere. Deltakere kan se åpne kurs, melde seg på og se eller avbestille egne påmeldinger. Instruktører kan opprette og administrere kurs de selv eier. Et kurs har en kapasitet, og en påmelding reserverer én plass. Samme deltaker kan ikke ha mer enn én aktiv påmelding på samme kurs, og kurset kan ikke overselges.

### Datamodell og ER-diagram

Datamodellen består av brukere, kurs og påmeldinger. Hvert kurs har én instruktør som eier det. En bruker kan ha flere påmeldinger, og hvert kurs kan ha flere påmeldinger. Hver påmelding knytter én bruker til ett kurs.

#### `kursplassen.users`

| Felt | Datatype | NULL tillatt |
|---|---|---|
| `id` | `char(36)` | Nei |
| `email` | `varchar(254)` | Nei |
| `password_hash` | `varchar(255)` | Nei |
| `display_name` | `varchar(120)` | Nei |
| `role` | `enum('Participant','Instructor')` | Nei |
| `created_at_utc` | `datetime(6)` | Nei |

#### `kursplassen.courses`

| Felt | Datatype | NULL tillatt |
|---|---|---|
| `id` | `char(36)` | Nei |
| `owner_user_id` | `char(36)` | Nei |
| `title` | `varchar(180)` | Nei |
| `description` | `text` | Nei |
| `starts_at_utc` | `datetime(6)` | Nei |
| `ends_at_utc` | `datetime(6)` | Nei |
| `location` | `varchar(180)` | Nei |
| `capacity` | `int unsigned` | Nei |
| `status` | `enum('Draft','Published','Cancelled')` | Nei |
| `created_at_utc` | `datetime(6)` | Nei |
| `updated_at_utc` | `datetime(6)` | Nei |

#### `kursplassen.registrations`

| Felt | Datatype | NULL tillatt |
|---|---|---|
| `id` | `char(36)` | Nei |
| `course_id` | `char(36)` | Nei |
| `participant_user_id` | `char(36)` | Nei |
| `registered_at_utc` | `datetime(6)` | Nei |
| `cancelled_at_utc` | `datetime(6)` | Ja |
| `active_participant_user_id` | `char(36)` | Ja |

#### Relasjoner

- Hvert kurs har én bruker som eier: `courses.owner_user_id` → `users.id`.
- Hver påmelding tilhører ett kurs: `registrations.course_id` → `courses.id`.
- Hver påmelding tilhører én deltaker: `registrations.participant_user_id` → `users.id`.
- En bruker kan eie flere kurs.
- En bruker kan ha flere påmeldinger.
- Et kurs kan ha flere påmeldinger.

ER-diagrammet er utgangspunktet for datamodellen. Tabeller, relasjoner og sentrale felter i implementasjonen skal følge diagrammet. Du kan velge database-first eller code-first med Entity Framework Core.

## Oppgave 1 – Implementering av REST API

### Formål

Utvikle et kjørbart API for kurskatalog og påmelding med autentisering, eierskapskontroll og en tydelig lagdeling.

### Obligatoriske krav

- Bruk ASP.NET Core Web API med controllere eller Minimal API, dependency injection og tjenestelag.
- Bruk separate request- og response-DTO-er. API-kontrakter skal ikke være databaseentiteter.
- Dokumenter API-et med Swagger/OpenAPI eller tilsvarende.
- Bruk MySQL og Entity Framework Core, relasjoner fra diagrammet og asynkrone databaseoperasjoner. Dokumenter databaseoppsett eller migreringer.
- Tillat registrering uten autentisering og bruk JWT for beskyttede endepunkter. Passord skal lagres med egnet hashing, aldri i klartekst.
- Bruk konto-rollene `Participant` og `Instructor`. En `CourseOwner` er en `Instructor` som eier det aktuelle kurset, og en `RegistrationOwner` er en `Participant` som eier den aktuelle påmeldingen. Dette er tilgangsbetegnelser, ikke egne konto- eller JWT-roller. Deltakere kan bare lese og avbestille egne påmeldinger. Instruktører kan bare endre eller slette kurs de eier.
- Valider innkommende data, bruk forståelige og konsistente feilresponser og implementer global feilhåndtering.
- Bruk strukturert logging uten passord, JWT-er eller andre hemmeligheter.
- Implementer health check og rate limiting for registrering eller innlogging. Begrunn valget kort.

### Påkrevde endepunkter

| Metode | Rute | Tilgang | Beskrivelse |
|---|---|---|---|
| POST | `/api/auth/register` | Ingen | Oppretter deltakerkonto; rollen settes av systemet. |
| POST | `/api/auth/login` | Ingen | Validerer innlogging og returnerer JWT. |
| GET | `/api/courses` | Ingen | Lister publiserte, fremtidige kurs. |
| GET | `/api/courses/{courseId}` | Ingen | Viser kursdetaljer, antall påmeldte og ledige plasser. |
| POST | `/api/courses` | Instructor | Oppretter kurs; innlogget instruktør blir eier. |
| PUT | `/api/courses/{courseId}` | CourseOwner | Oppdaterer eget kurs. |
| DELETE | `/api/courses/{courseId}` | CourseOwner | Sletter kurs uten aktive påmeldinger. |
| POST | `/api/courses/{courseId}/registrations` | Participant | Melder innlogget deltaker på kurset. |
| GET | `/api/registrations/mine` | Participant | Viser egne påmeldinger med kursinformasjon. |
| DELETE | `/api/registrations/{registrationId}` | RegistrationOwner | Avbestiller egen påmelding og frigjør plassen. |
| GET | `/health` | Ingen | Rapporterer helsetilstand. |

### Funksjonelle krav

1. En deltaker kan registrere seg og logge inn. Brukeridentitet og rolle skal hentes fra autentisert kontekst, ikke stoles på fra request-body.
2. Kurs skal ha tittel, beskrivelse, starttid, sluttid, sted og kapasitet. Tittel og sted kan ikke være tomme, sluttid må være etter starttid, og kapasitet må være minst én.
3. Kurslisten viser bare publiserte, fremtidige kurs. Kursdetaljer viser kapasitet og tilgjengelige plasser uten sensitive brukeropplysninger.
4. En instruktør kan opprette kurs og lese, endre eller slette bare kurs vedkommende eier.
5. En deltaker kan melde seg på et publisert kurs som ikke har startet, hvis det er ledig kapasitet. Samtidige forespørsler skal ikke overselge kurset.
6. Deltakeren kan ikke ha to aktive påmeldinger på samme kurs. Påmelding på fullt, startet eller ikke-publisert kurs avvises med egnet status.
7. Deltakere kan bare se og avbestille egne påmeldinger. Avbestilling frigjør plassen. Kurs med aktive påmeldinger kan ikke slettes.
8. Manglende ressurser og ugyldige felt gir forståelige feilresponser uten stack trace eller intern informasjon.

### Leveranser og vurdering

Lever et kjørbart API med datamodell, databaseoppsett, dokumentasjon, testdata og endepunktene over. Det vurderes funksjonalitet, REST-praksis, dataintegritet, arkitektur, sikkerhet, validering, feilhåndtering, logging, health check og rate limiting.

### Valgfri videreutvikling

Filtrering, sortering eller paginering kan legges til, men erstatter ikke obligatoriske krav.

## Oppgave 2 – Testing og dokumentasjon

Skriv minst to relevante enhetstester med xUnit for forretningslogikk eller validering. Bruk mocking eller stubbing der det er relevant. Skriv minst én integrasjonstest av API-et uten produksjonsdatabase.

Lever en kort `README.md` med prosjektoversikt, forutsetninger, instruksjoner for å kjøre løsningen, databaseoppsett eller migreringer, testkommando, valg av database-first eller code-first og KI-dokumentasjon eller KI-erklæring. Legg ved eksempelkall i en `.http`-fil, Postman, curl eller tilsvarende for registrering, innlogging, kursflyt, påmelding, en autorisasjonsfeil og en valideringsfeil.

Det vurderes om testene er relevante, integrasjonstesten dekker API-grensen, og dokumentasjonen gjør løsningen mulig å starte og prøve.

## Oppgave 3 – Videopresentasjon og refleksjon

Lag en video på 8–12 minutter med skjermdeling og tydelig lyd. Webcam er valgfritt. Videoen kan være på norsk eller engelsk. Vis API-et i bruk og forklar autentisering og eierskap, datamodell og relasjoner, sentrale endepunkter, arkitektur og DTO-er, validering, feilhåndtering, logging, minst én enhetstest og integrasjonstest, én utfordring og mulig forbedring. Ta med kort refleksjon over KI-bruk dersom KI er brukt.

Det vurderes om demonstrasjonen viser relevante funksjoner, forklaringene er faglig presise, og du viser forståelse for egne valg og begrensninger.

## Innlevering

Lever én ZIP-fil med beskrivende filnavn. ZIP-filen skal inneholde komplett .NET-løsning med kildekode, testprosjekt, `README.md`, nødvendige data eller databaseutvidelser, API-eksempler, KI-dokumentasjon eller erklæring og videofil eller tilgjengelig videolenke. Offentlig repository er ikke påkrevd.

## Læringsutbytter og vurderingskriterier

Eksamen vurderer kunnskap om HTTP, JSON og RDBMS fra programkode; evne til å bruke backend-rammeverk og software design-prinsipper; evne til å utvikle og teste REST API; bruk av RDBMS via ORM; vurdering av sikkerhet; og evne til å begrunne og presentere API-løsninger.

| Område | Hva vurderes |
|---|---|
| Analyse og design | Forståelse av datamodell og faglige implementasjonsvalg |
| API-funksjonalitet | REST, CRUD, HTTP-metoder, statuskoder og DTO-er |
| Datatilgang | EF Core, MySQL, relasjoner, asynkronitet og integritet |
| Sikkerhet og robusthet | JWT, hashing, eierskap, validering og feilhåndtering |
| Kvalitet og drift | Struktur, logging, health check og rate limiting |
| Testing | Relevante enhets- og integrasjonstester |
| Dokumentasjon og formidling | README, eksempler, video og KI-refleksjon |

Vurderingen er helhetlig. Mangler i grunnleggende sikkerhet, sentral funksjonalitet, testbarhet eller dokumentasjon begrenser karakteren selv om andre deler er sterke.

## Vedlegg 1: Forslag til kurs

Her er et forslag til kurs som kan brukes som testdata i databasen `kursplassen`.

### Testdata

#### Brukere

| Visningsnavn (`display_name`) | E-post (`email`) | Rolle (`role`) |
|---|---|---|
| Ida Hansen | ida@example.com | Instructor |
| Omar Ali | omar@example.com | Instructor |
| Nora Berg | nora@example.com | Participant |
| Emil Olsen | emil@example.com | Participant |
| Sara Nilsen | sara@example.com | Participant |
| Jonas Lund | jonas@example.com | Participant |

#### Kurs

| Tittel (`title`) | Eier | Start (`starts_at_utc`) | Slutt (`ends_at_utc`) | Sted (`location`) | Kapasitet (`capacity`) | Status (`status`) |
|---|---|---|---|---|---|---|
| Grunnleggende Python | Ida Hansen | 2026-12-15 09:00 | 2026-12-15 14:00 | Sandefjord, rom A | 12 | Published |
| Introduksjon til SQL | Omar Ali | 2026-12-16 09:00 | 2026-12-16 12:00 | Nettbasert | 2 | Published |
| REST API med ASP.NET Core | Ida Hansen | 2026-12-17 09:00 | 2026-12-17 15:00 | Sandefjord, rom B | 10 | Draft |
| Git og GitHub | Omar Ali | 2026-12-18 09:00 | 2026-12-18 12:00 | Nettbasert | 8 | Cancelled |
| Docker for utviklere | Omar Ali | 2026-11-10 09:00 | 2026-11-10 14:00 | Sandefjord, rom A | 10 | Published |
| Sikkerhet i webapplikasjoner | Ida Hansen | 2026-12-21 09:00 | 2026-12-21 13:00 | Nettbasert | 6 | Published |

#### Påmeldinger

| Kurs | Deltaker | Påmelding |
|---|---|---|
| Grunnleggende Python | Nora Berg | Aktiv |
| Grunnleggende Python | Emil Olsen | Aktiv |
| Introduksjon til SQL | Nora Berg | Aktiv |
| Introduksjon til SQL | Sara Nilsen | Aktiv |
| Sikkerhet i webapplikasjoner | Jonas Lund | Avbestilt |

### Forslag til testing

Med disse dataene kan du demonstrere at:

1. Kurslisten viser publiserte, fremtidige kurs.
2. Kursdetaljer viser riktig antall aktive påmeldinger og ledige plasser.
3. Introduksjon til SQL er fullbooket, og nye påmeldinger avvises.
4. En deltaker ikke kan ha to aktive påmeldinger på samme kurs.
5. Avbestilling frigjør en plass, og deltakeren kan melde seg på igjen.
6. Påmelding til utkast, avlyste eller startede kurs avvises.
7. Deltakere bare kan se og avbestille egne påmeldinger.
8. Instruktører bare kan endre eller slette egne kurs.
9. Kurs med aktive påmeldinger ikke kan slettes.
10. Samtidige påmeldingsforsøk til siste ledige plass ikke overselger kurset.