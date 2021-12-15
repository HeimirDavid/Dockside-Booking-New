# ghrdev-bokningssystem
Bokningssystem för Dockside Kontorshotell AB

Dockside Kontorshotell AB driver sedan flera år tillbaka kontorshotellsverksamhet och efter
en större expansion under 2019-2020 vilar uthyrningsverksamheten på två ben,
kontorsplatser och möteslokaler - Office & Coworking respektive Meetings.

Förutom mindre mötesrum i anslutning till kontorslokalerna, finns en samlat mötescentra
med lokaler såväl för det mindre mötet som det mer krävande kundeventet, konferensen
eller den större utbildningsgruppen.

Bokningsprocessen har tills dags datum varit helt manuellt betingad och behovet av ett
digitalt bokningssystem är med de nya lokalerna överhängande. Genom att tillgängliggöra
bokningen vill vi skapa enkelhet, effektivitet och användarvänlighet för våra kunder och
därmed öka beläggningsgraden.

Nedan listas de grundkrav och önskemål som redan identifierats kring ett nytt system under
ett antal delområden. Vi vill även uppmana till att göra en genomlysning av redan
existerande bokningssystem med fokus på möteslokaler. Vilka smarta features,
upplevelsehöjande funktioner eller rentav nödvändiga säkerhetsaspekter finns där ute idag
och kan/måste/borde tas med i vårt projekt?

==================================================================================================

TEKNISK KOMPABILITET
Dagens hemsida (docksideoffice.se) bygger på Wordpress och förvaltas under ett konto hos
Loopia. Ett nytt bokningssystem behöver kunna integreras med dessa plattformar.
Hemsidan är dock föremål för uppdatering/ombyggnad varför alltför stor vikt inte behöver
läggas på den befintliga layouten, typsnitt etc. Med det sagt så behöver följande allmänna
grundkrav uppfyllas:

- Fullt integrerbart i Wordpress och Loopia-miljön och nåbart från hemsidan, under
docksideoffice.se/bookings eller liknande.
- Uppbyggt enligt allmänt vedertagna metodiker/standarder. En öppen lösning,
framtidssäkrad för påbyggnad eller vidareutveckling.
- Ett generiskt system tillgängligt för alla, ej kopplat just till t ex Outlook eller annan
specifik programvara.
- Användargränssnittet måste ha fullgod synlighet och användbarhet på alla vanligt
förekommande plattformar/skärmar.
- Gränssnittet måste gå att ”Docksideifiera”, varumärkesanpassa, variera i färg,
typsnitt etc.
- Eventuella plug-ins eller liknande som används bör vara utan löpande kostnader.
- Inloggningsförfarande bör kopplas till relevant GDPR-information, att samtycke
inhämtas för hantering av uppgifter.

==================================================================================================

GRUNDLÄGGANDE FUNKTIONALITET
Bokningssystemet skall kunna nyttjas såväl av kontorshotellets egna hyresgäster som av
kunder utanför Docksides väggar. I första hand gäller behovet möteslokaler, men det skall
även gå att lägga till annat i bokningsflödet (skrivbordsplatser, elcyklar, poolbil eller
liknande).
Med tanke på interna/externa gäster så ser jag framför mig två användarnivåer. Nivåer som
skulle kunna få olika bokningsobjekt presenterade för sig, olika priser angivna och kanske
olika regelverk för avbokning? Olika behörighetstyper skulle kunna läggas i ett steg 2, men
inkluderas företrädelsevis i en första lösning. Systemet skall åtminstone utformas med detta
i åtanke. Därtill förstås en administratörsfunktion där vi själva kan uppdatera all information
i realtid.

==================================================================================================

BOKNING
- Varje bokningsobjekt ska kunna presenteras med beskrivning, nyckeldata, bild(er)
och pris.
- Admin väljer synlighet och pris per användarnivå.
- Bokning ska kunna göras i en datumkalender på hela klockslag och varje halv timma.
- Heldag skall generera bättre pris… Antingen genom eget bokningsslag eller att
debiteringen aldrig överstiger XYZ (motsvarande 6 timmar exempelvis).
- Möjlighet att göra tillval till en bokning, t ex cateringservice.
- Möjlighet att lämna meddelande tillsammans med bokningen.
- ”Begär offert”-knapp skall finnas för den som är osäker eller vill göra en större
bokning. Leder till formulär alternativt mail.
- Möjlighet till automatgenererade bekräftelsemail skall finnas.
- Möjlighet till notifieringsmail till admin skall finnas. Valbart per användarnivå och
bokningsobjekt.
- Avbokningsregler skall kunna sättas per användarnivå. T ex avbokning senare än
tidpunkt X ger Y % debitering. Ej möjligt att ta bort bokning i efterhand.
PRESENTATION
- De olika möteslokalerna bör, förutom i någon form av listvy, även kunna presenteras
visuellt i en mer överskådlig form. T ex via en planritning där tillgängligheten på
respektive lokal åskådliggörs via röd/grön färg och där varje bokningsbar lokal är
klickbar. Just planritningen skulle även ge en igenkänning och en bekräftelse över att
man verkligen bokar rätt lokal.
- Om möjligt hade det varit värdefullt att kunna ge en översiktsbild över bokningsläget
för tillfället, även oinloggat. Jag tänker på den som söker ett ledigt rum ”här och nu”
på plats på kontoret. Då skulle det t ex alltid kunna finnas en ögonblicksbild tillgänglig
på en centralt placerad touchskärm.

==================================================================================================

RAPPORTER
- Det måste gå att generera, och exportera, rapporter över bokningar. För analys och
som underlag för fakturering.
- Rapportering per tidsenhet och per företag. Summerat och specificerat per kund och
vald period.
FUNDERINGAR, UTMANINGAR – NICE TO HAVES?
1. QR-kod vid respektive rum…
Det finns idag fina (och dyra) touchskärmar, speciellt utvecklade för ändamålet, att sätta
utanför varje möteslokal som direkt visar på tillgängligheten för stunden och den närmaste
tiden framåt, samtidigt som de ger möjlighet till direktbokning.
Jag kan tycka (oavsett kostnad) att det är lite overkill för mina lokaler och mina behov i
dagsläget och kräver även strömförsörjning till varje dörr, vilket inte finns framme idag.
Dock tilltalar tanken om att enkelt kunna få information om en viss lokal när man väl står
precis utanför och undrar… information om vad som ingår, vad det kostar och förstås om det
är ledigt. Där föddes tanken om en QR-kod. Läs koden, bli direktlänkad till just detta
rummets sida, och boka om du vill. Kan möjligen bli något av en utmaning att få enkelhet i,
med tanke på inloggning och min idé om olika användarnivåer, men värt att fundera kring.
2. Digital nyckel
Det finns bokningssystem på marknaden som kan integreras med olika passagesystem, varpå
en bokning av en lokal automatiskt genererar en digital nyckel som aktiveras under tiden för
bokningen. Detta är flott och något som sannolikt med tiden kommer att bli mer av
hygiennivå än cool feature. Vi har ej stödet för digitala nycklar idag, men avser att skaffa och
går det att bygga standardiserat och förberedande för detta så vore det super!
3. Fria timmar
En tanke finns om att kunna erbjuda våra fasta hyresgäster X fria timmar per månad i
utvalda mötesrum. Detta skulle då behöva följas, avräknas och rapporteras genom
bokningssystemet, för berörda användare. På lämpligt sätt J.
4. App…
Skulle alltsammans göra sig bra i en mobilapp… eller inte?

==================================================================================================

SUMMERING OCH MEDSKICK
Dockside Kontorshotell är ett litet entreprenörsdrivet företag som vill vara det prisvärda och
personliga alternativet med stor flexibilitet och kundens behov i fokus. Vår arena är
Lindholmen Science Park, regionens kanske allra hetaste områden för teknik och innovation.
En arena där vi på senare tid har kommit att slåss om kunderna med internationella giganter
inom coworking och aktörer helägda av landets större fastighetsbolag.
Här verkar vi under parollen ”Smartare Kontor – Bättre Möten”. Med det menar vi, väldigt
grovt, att vi erbjuder allt det du som företagskund behöver, men kanske inte allt du INTE
behöver. Detta kopplat till en för området klart attraktiv prisbild.
Således behöver inte allt vi gör vara top notch, men det måste vara bra, och gärna tekniskt
smart. Och enkelt.
Jag vill slå ett extra slag för enkelheten. Idag bokar vi möten på en lista utanför varje rum.
Det är jätteenkelt! Man ser direkt om det är ledigt och man kan boka in sig på nolltid. Nu
känns det dock förlegat och med ökad digitalisering, ökad flexibilitet i arbetssätt så har ett
bokningssystem hög prio för oss. Vi måste kunna se tillgänglighet och boka när vi vill och var
vi än befinner oss.
Nu ska vi även lära de som normalt bokar på lappen utanför rummet, och tycker att det
fungerar bra, att boka online. Och de måste lära sig. Inte minst för denna omställning är
enkelheten väldigt viktig.
