# Kursplassen — sjekkliste fra oppstart til innlevering

Sjekklisten organiserer eksamenskravene i arbeidsrekkefølge, fra planlegging til ferdig ZIP-fil. Punkter merket «forslag» er arbeidsstøtte, ikke tilleggskrav i oppgaven. Innleveringsfrist: 11. desember 2026.

## 1. Avklar krav og planlegg løsningen

- [x] Les hele oppgaveteksten, inkludert ER-diagrammet og vurderingskriteriene.
- [x] Lag en oversikt over obligatoriske endepunkter.
- [x] Velg controllere eller Minimal API.
- [x] Velg database-first eller code-first med Entity Framework Core.
- [x] Planlegg tydelig lagdeling med API-lag, tjenestelag og datatilgang.
- [ ] Planlegg separate request- og response-DTO-er.
- [ ] Avklar hvordan instruktørkontoer skal gjøres tilgjengelige i testdata; offentlig registrering skal bare opprette `Participant`.
- [ ] Noter uklarheter som må avklares eller dokumenteres:
    - [ ] Hvordan offentlig lesing av kursdetaljer skal forstås opp mot kravet om at instruktører bare kan lese egne kurs.
    - [ ] Hvordan upubliserte kurs skal være tilgjengelige for eieren.
    - [ ] Hvordan sletting av kurs med bare avbestilte påmeldinger skal håndteres.
- [ ] Opprett et løpende notat om KI-bruk, egne vurderinger og endringer.
- [x] Forslag: Opprett Git-repository og del arbeidet inn i små milepæler.

## 2. Opprett løsning og grunnstruktur

- [x] Opprett ASP.NET Core Web API-prosjekt.
- [x] Opprett testprosjekt med xUnit.
- [x] Legg prosjektene til i samme .NET-løsning.
- [ ] Opprett struktur for:
    - [x] Endepunkter eller controllere.
    - [ ] Request- og response-DTO-er.
    - [ ] Tjenester og forretningslogikk.
    - [ ] Entiteter og databasekonfigurasjon.
    - [ ] Autentisering og autorisasjon.
    - [ ] Validering og global feilhåndtering.
- [ ] Konfigurer dependency injection.
- [ ] Sett opp konfigurasjon for MySQL og JWT.
- [ ] Forslag: Hold faktiske hemmeligheter utenfor filer som skal leveres.
- [x] Kontroller at løsningen bygger og API-et starter.

## 3. Implementer datamodell og database

### Brukere — `users`

- [x] Opprett feltene fra ER-diagrammet:
    - [x] `id`
    - [x] `email`
    - [x] `password_hash`
    - [x] `display_name`
    - [x] `role`
    - [x] `created_at_utc`
- [x] Støtt konto-rollene `Participant` og `Instructor`.

### Kurs — `courses`

- [x] Opprett feltene fra ER-diagrammet:
    - [x] `id`
    - [x] `owner_user_id`
    - [x] `title`
    - [x] `description`
    - [x] `starts_at_utc`
    - [x] `ends_at_utc`
    - [x] `location`
    - [x] `capacity`
    - [x] `status`
    - [x] `created_at_utc`
    - [x] `updated_at_utc`
- [x] Støtt kursstatusene `Draft`, `Published` og `Cancelled`.

### Påmeldinger — `registrations`

- [x] Opprett feltene fra ER-diagrammet:
    - [x] `id`
    - [x] `course_id`
    - [x] `participant_user_id`
    - [x] `registered_at_utc`
    - [x] `cancelled_at_utc`
    - [x] `active_participant_user_id`

### Relasjoner og integritet

- [x] Knytt hvert kurs til én eier.
- [x] Knytt hver påmelding til ett kurs.
- [x] Knytt hver påmelding til én deltaker.
- [ ] Konfigurer datatyper, feltlengder og nullable-felter i samsvar med diagrammet.
- [ ] Implementer en løsning som hindrer flere aktive påmeldinger for samme deltaker på samme kurs.
- [ ] Planlegg hvordan samtidige påmeldinger skal håndteres uten oversalg.
- [ ] Planlegg hvordan avbestilling, kursoppdatering og sletting skal bevare dataintegriteten.
- [ ] Bruk asynkrone databaseoperasjoner.
- [ ] Opprett migreringer eller nødvendige databaseoppsettsfiler.
- [ ] Kontroller at databasen kan opprettes fra dokumenterte instruksjoner.

## 4. Lag felles API-kontrakter og feilhåndtering

- [ ] Definer request-DTO-er for registrering, innlogging og kursoppretting/-oppdatering.
- [ ] Definer response-DTO-er for autentisering, kurs og påmeldinger.
- [ ] Sørg for at databaseentiteter ikke brukes som API-kontrakter.
- [ ] Definer konsistente feilresponser.
- [ ] Implementer global feilhåndtering.
- [ ] Sørg for at feilresponser ikke inneholder stack trace eller intern informasjon.
- [ ] Implementer validering av innkommende data.
- [ ] Kontroller at ugyldige felt gir forståelige feilmeldinger.

## 5. Implementer registrering og innlogging

### Registrering

- [ ] Implementer `POST /api/auth/register`.
- [ ] Tillat registrering uten autentisering.
- [ ] Sett rollen til `Participant` på serversiden.
- [ ] Sørg for at klienten ikke kan velge `Instructor` gjennom registreringsforespørselen.
- [ ] Valider registreringsdata.
- [ ] Hash passord med egnet passordhashing.
- [ ] Kontroller at passord aldri lagres i klartekst.
- [ ] Returner en response-DTO uten passord eller passordhash.

### Innlogging og JWT

- [ ] Implementer `POST /api/auth/login`.
- [ ] Valider innloggingsinformasjon.
- [ ] Returner JWT ved gyldig innlogging.
- [ ] Konfigurer JWT-autentisering for beskyttede endepunkter.
- [ ] Hent brukeridentitet og rolle fra autentisert kontekst.
- [ ] Ikke stol på bruker-ID eller rolle fra request-body.
- [ ] Kontroller at manglende eller ugyldig autentisering avvises.

## 6. Implementer kurskatalogen

### Offentlig lesing

- [ ] Implementer `GET /api/courses`.
- [ ] Vis bare publiserte kurs som ikke har startet.
- [ ] Implementer `GET /api/courses/{courseId}`.
- [ ] Returner kursdetaljer med:
    - [ ] Kapasitet.
    - [ ] Antall aktive påmeldinger.
    - [ ] Antall ledige plasser.
- [ ] Sørg for at kursdetaljer ikke eksponerer sensitive brukeropplysninger.
- [ ] Håndter kurs som ikke finnes med en forståelig feilrespons.
- [ ] Implementer og dokumenter den avklarte tilgangen til upubliserte kurs.

### Validering av kursdata

- [ ] Tittel kan ikke være tom.
- [ ] Sted kan ikke være tomt.
- [ ] Sluttid må være etter starttid.
- [ ] Kapasitet må være minst én.
- [ ] Kurs inneholder beskrivelse, starttid, sluttid, sted og kapasitet.
- [ ] Feltlengder samsvarer med datamodellen.

## 7. Implementer instruktørfunksjoner og eierskap

- [ ] Implementer `POST /api/courses`.
- [ ] Begrens oppretting til `Instructor`.
- [ ] Sett innlogget instruktør som eier på serversiden.
- [ ] Implementer `PUT /api/courses/{courseId}`.
- [ ] Tillat bare eieren å oppdatere kurset.
- [ ] Implementer `DELETE /api/courses/{courseId}`.
- [ ] Tillat bare eieren å slette kurset.
- [ ] Avvis sletting av kurs med aktive påmeldinger.
- [ ] Håndter sletting av kurs med bare avbestilte påmeldinger i tråd med valgt løsning.
- [ ] Sørg for at `CourseOwner` er en eierskapskontroll, ikke en egen konto- eller JWT-rolle.
- [ ] Kontroller at én instruktør ikke kan endre eller slette en annen instruktørs kurs.

## 8. Implementer påmelding og avbestilling

### Påmelding

- [ ] Implementer `POST /api/courses/{courseId}/registrations`.
- [ ] Begrens endepunktet til `Participant`.
- [ ] Hent deltakerens identitet fra autentisert kontekst.
- [ ] Tillat bare påmelding når:
    - [ ] Kurset finnes.
    - [ ] Kurset er publisert.
    - [ ] Kurset ikke har startet.
    - [ ] Det finnes ledig kapasitet.
    - [ ] Deltakeren ikke allerede har en aktiv påmelding.
- [ ] Returner forståelige feil med egnet status ved avvisning.
- [ ] Sikre at samtidige forespørsler ikke overselger kurset.
- [ ] Sikre at samtidige forespørsler fra samme deltaker ikke oppretter to aktive påmeldinger.

### Egne påmeldinger

- [ ] Implementer `GET /api/registrations/mine`.
- [ ] Begrens endepunktet til `Participant`.
- [ ] Returner bare innlogget deltakers egne påmeldinger.
- [ ] Inkluder relevant kursinformasjon.

### Avbestilling

- [ ] Implementer `DELETE /api/registrations/{registrationId}`.
- [ ] Tillat bare deltakeren som eier påmeldingen å avbestille.
- [ ] Sørg for at avbestilling frigjør plassen.
- [ ] Sørg for at deltakeren kan melde seg på igjen etter avbestilling.
- [ ] Sørg for at `RegistrationOwner` er en eierskapskontroll, ikke en egen konto- eller JWT-rolle.

## 9. Implementer logging, health check og rate limiting

- [x] Implementer strukturert logging.
- [ ] Kontroller at logger ikke inneholder:
    - [ ] Passord.
    - [ ] Passordhash.
    - [ ] JWT-er.
    - [ ] Andre hemmeligheter.
- [x] Implementer offentlig `GET /health`.
- [ ] Implementer rate limiting på registrering eller innlogging.
- [ ] Skriv en kort begrunnelse for valget av rate limiting.
- [ ] Kontroller at rate limiting faktisk aktiveres ved gjentatte forespørsler.
- [ ] Kontroller at globale feil håndteres og logges uten å lekke interne detaljer til klienten.

## 10. Legg inn testdata og prøv hele flyten

- [ ] Gjør instruktørkontoer tilgjengelige for demonstrasjon.
- [ ] Gjør deltakerkontoer tilgjengelige for demonstrasjon.
- [ ] Legg inn kurs som dekker:
    - [ ] Publisert, fremtidig kurs med ledige plasser.
    - [ ] Fullbooket kurs.
    - [ ] Utkast.
    - [ ] Avlyst kurs.
    - [ ] Startet eller tidligere kurs.
- [ ] Legg inn aktive og avbestilte påmeldinger.
- [ ] Dokumenter hvordan testdata opprettes.
- [ ] Kontroller datoene slik at demonstrasjonen viser riktige tilfeller.
- [ ] Kjør manuelt gjennom:
    - [ ] Registrering og innlogging.
    - [ ] Visning av kursliste og kursdetaljer.
    - [ ] Oppretting og oppdatering av eget kurs.
    - [ ] Avvisning av endring av en annens kurs.
    - [ ] Påmelding.
    - [ ] Avvisning av dobbel aktiv påmelding.
    - [ ] Avvisning av påmelding til fullt, startet eller upublisert kurs.
    - [ ] Visning av egne påmeldinger.
    - [ ] Avbestilling og ny påmelding.
    - [ ] Avvisning av avbestilling av en annens påmelding.
    - [ ] Avvisning av sletting av kurs med aktive påmeldinger.
    - [ ] Sletting av kurs uten aktive påmeldinger.
    - [ ] Samtidige påmeldingsforsøk til siste ledige plass.

## 11. Skriv og kjør automatiserte tester

Testene kan utvikles underveis; dette er kontrollpunktet for å sikre at testleveransen er komplett.

### Obligatoriske tester

- [ ] Skriv minst to relevante enhetstester med xUnit for forretningslogikk eller validering.
- [ ] Bruk mocking eller stubbing der det er relevant.
- [ ] Skriv minst én integrasjonstest av API-et.
- [ ] Sørg for at integrasjonstesten dekker API-grensen.
- [ ] Sørg for at integrasjonstesten ikke bruker produksjonsdatabase.
- [ ] Kjør alle tester og kontroller at de består.

### Forslag til ekstra testdekning

- [ ] Ugyldig sluttid avvises.
- [ ] Kapasitet under én avvises.
- [ ] Påmelding til fullt kurs avvises.
- [ ] Dobbel aktiv påmelding avvises.
- [ ] Avbestilling frigjør kapasitet.
- [ ] Eierskapskontroll avviser feil bruker.
- [ ] Samtidige påmeldinger fører ikke til oversalg.

## 12. Fullfør API-dokumentasjon og eksempelkall

- [ ] Dokumenter API-et med Swagger/OpenAPI eller tilsvarende.
- [ ] Kontroller at alle obligatoriske endepunkter er dokumentert.
- [ ] Kontroller at request- og response-kontrakter stemmer med implementasjonen.
- [ ] Legg ved eksempelkall i `.http`-fil, Postman, curl eller tilsvarende for:
    - [ ] Registrering.
    - [ ] Innlogging.
    - [ ] Kursflyt.
    - [ ] Påmelding.
    - [ ] En autorisasjonsfeil.
    - [ ] En valideringsfeil.
- [ ] Kjør eksempelkallene og kontroller at de fungerer.

## 13. Fullfør README og KI-refleksjon

### README

- [ ] Beskriv prosjektet og funksjonaliteten.
- [ ] Oppgi forutsetninger for å kjøre løsningen.
- [ ] Beskriv nødvendig konfigurasjon.
- [ ] Dokumenter databaseoppsett eller migreringer.
- [ ] Oppgi om løsningen bruker database-first eller code-first.
- [ ] Dokumenter hvordan API-et startes.
- [ ] Dokumenter hvordan testene kjøres.
- [ ] Beskriv hvordan testdata og instruktørkontoer gjøres tilgjengelige.
- [ ] Forklar kort valget av rate limiting.
- [ ] Dokumenter relevante avklaringer, valg og begrensninger.
- [ ] Inkluder KI-refleksjon eller henvis til egen fil.

### KI-refleksjon

- [ ] Beskriv hvordan KI ble brukt.
- [ ] Beskriv hva KI bidro med.
- [ ] Forklar hvordan KI påvirket den endelige løsningen.
- [ ] Beskriv hva du selv vurderte, endret eller videreutviklet.
- [ ] Beskriv feil, begrensninger eller utfordringer ved KI-bruken.
- [ ] Kontroller at refleksjonen samsvarer med faktisk bruk.

## 14. Planlegg og spill inn video

- [ ] Lag en disposisjon for en video på 8–12 minutter.
- [ ] Klargjør skjermdeling og kontroller lyd.
- [ ] Velg norsk eller engelsk.
- [ ] Demonstrer API-et i bruk.
- [ ] Forklar:
    - [ ] Autentisering og JWT.
    - [ ] Roller og eierskapskontroll.
    - [ ] Datamodell og relasjoner.
    - [ ] Sentrale endepunkter.
    - [ ] Arkitektur og tjenestelag.
    - [ ] Request- og response-DTO-er.
    - [ ] Validering.
    - [ ] Global feilhåndtering.
    - [ ] Logging.
- [ ] Vis og forklar minst én enhetstest.
- [ ] Vis og forklar minst én integrasjonstest.
- [ ] Beskriv én utfordring og en mulig forbedring.
- [ ] Ta med en kort refleksjon over KI-bruk.
- [ ] Kontroller at videoen viser egen forståelse av løsningen.
- [ ] Se gjennom opptaket og kontroller lengde, lyd og lesbarhet.
- [ ] Kontroller at eventuell videolenke er tilgjengelig for sensor.

## 15. Gjennomfør sluttkontroll fra en ren kopi

- [ ] Forslag: Pakk ut eller kopier løsningen til en ny mappe.
- [ ] Følg README uten å bruke udokumenterte lokale innstillinger.
- [ ] Kontroller at avhengigheter kan gjenopprettes.
- [ ] Kontroller at løsningen bygger.
- [ ] Opprett databasen fra dokumenterte instruksjoner.
- [ ] Legg inn testdata fra dokumenterte instruksjoner.
- [ ] Start API-et.
- [ ] Kjør alle automatiserte tester.
- [ ] Kjør API-eksemplene.
- [ ] Kontroller `/health`.
- [ ] Kontroller autentisering, roller og eierskap.
- [ ] Kontroller at feilresponser og logger ikke lekker hemmeligheter.
- [ ] Kontroller at du kan forklare hele løsningen og sentrale faglige valg.

## 16. Pakk og lever

- [ ] Lag én ZIP-fil med beskrivende filnavn.
- [ ] Inkluder:
    - [ ] Komplett .NET-løsning.
    - [ ] All nødvendig kildekode.
    - [ ] Testprosjekt.
    - [ ] `README.md`.
    - [ ] Migreringer eller databaseoppsett.
    - [ ] Nødvendige data eller databaseutvidelser.
    - [ ] API-eksempler.
    - [ ] KI-dokumentasjon eller KI-erklæring.
    - [ ] Videofil eller tilgjengelig videolenke.
- [ ] Forslag: Fjern unødvendige byggefiler og lokale hemmeligheter før pakking.
- [ ] Åpne ZIP-filen og kontroller innholdet.
- [ ] Kontroller videofilen eller videolenken én siste gang.
- [ ] Lever innen 11. desember 2026.
- [ ] Kontroller at innleveringen er registrert.