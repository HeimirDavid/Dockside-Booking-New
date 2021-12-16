# ghrdev-bokningssystem
Bokningssystem för Dockside Kontorshotell AB

Projekt byggt av tre studenter från SKY i järfälla där vårt uppdrag var att bygga ett bokningssystem för ett kontorshotell under 6 veckor.
Då vi studerar till att bli .NET utvecklare var valet ganska enkelt om vilken code stack vi skulle använda.
**Projektet ligger uppe på följande länk:** https://ghdev.maxdev.se/


Ska tilläggas att det här är ett internt system primärt och då krävs inloggning för att ta sig in.
Därför finns en bildgenomgång här:
## Inloggad Användare

### Mina Bokningar
Sida där användaren kan se sina bokningar och ändra / avbryta en bokning. Användarens historik finns också tillgänglig här.

![Mina bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.57.59.png)

### Skapa Bokning
I en kalendervy här kan en användare skapa en bokning tillhörande ett rum som hen valt. Detta görs genom en "drag and drop" i kalendern. Andra bokningar tillhörande rummet 
syns också för att skapa en frontend validering för att minska risken för en dubbelbokning.
  
![Mina bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.58.28.png)
  
![Mina bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.58.53.png)


## Admin Panel
### Startsida
Här kan admin direkt se dagens och morgondagens bokningar.
![Mina bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.59.33.png)
![Mina bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.59.24.png)

### Alla bokningar
Här finns en listvy av alla bokningar i systemet. Med hjälp av DataTables är denna tabell sökbar på många sätt. Det går att kombinera år och månad tillsammans med företagsnamn för att t.ex. få fram alla bokningar för företaget för att sedan exportera till en PDF, exempelvis som underlag för rapporter. Här finns även länkar till att ändra en bokning, ändra priset manuellt eller avbryta en bokning. 

![Alla bokningar](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.59.48.png)
  
Pris ändring:

![Pris](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.59.55.png)

### Alla användare
Lista över alla användare med DataTables på samma sätt som bokningarna. Här går också att sortera, filtrera och exportera. Härifrån kan du inaktivera användare och ändra dess uppgifter. Denna funktionalitet finns även hos rum och företag.

![Alla användare](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2018.59.55.png)
  
Filtreringsexpempel på alla bokningar gjorda november 2021
  
![Alla användare](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2019.01.37.png)

### Rumsdetaljer
Lite mer beskrivande vy av rummet där all info är samlad samt bild. Denna vy finns också hos företag och användare.

![Rumsdetaljer](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2019.00.45.png)

### Skapa ny användare
Här skapar man en ny användare. Fungerar liknande och ser likadant ut för rum och företag-

![Rumsdetaljer](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2019.00.56.png)
  
![Rumsdetaljer](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2019.01.09.png)

### Företagshistorik

En vy som visar ett företags bokningshistorik. Också presenterat med DataTables med alla dess funktioner.
![Rumsdetaljer](https://github.com/HeimirDavid/Dockside-Booking-New/blob/main/05%20Bildspel/Screenshot%202021-12-16%2019.02.23.png)













