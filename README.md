# Perustakaa kahen tai kolmen hengen projektiryhmä(t).

# SÄHKÖNKÄYTTÄJÄN MAAILMA -SIMULAATTORI
- Pohtikaa alla olevia "englanninkielisiä" esimerkkejä.
- Testatkaa tekoalyn käyttämistä ensimmäisten prototyyppien luomiseen.
- Perustakaa GIT-repo projektillenne tallentamista ja yhetistyötä varten.
- Tehkää projektisuunnitelma (.doc) ja jakaa työt ryhmän jäsenille.
- Pitäkää projektipalavereita (2 per viikko) ja
- Kirjatkaa pöytäkirjoihin (.doc) edistyminen ja muutokset edellä olevaan projektisuunnitelmaan.
- Koodatkaa määritellyt osat yksin tai/ja yhdessä,
- Testatkaa yksin ja yhdessä.
- Dokumentoikaa virheet, niiden korjaukset ja kirjatkaa muutokset suunnitelmaan.
- Käyttäkää Googlea ja tekoälyä, ottakaa selvää sähkön käyttämiseen liittyvästä sanastosta
- Miettikää voiko sää olla 'random', mihin tarvitaan tietokantaa, ...
- Mitä saatte aikaiseksi päivässä, viikossa, vajaassa kuukaudessa

# Arviointi:
- Ideointi (Alkuperäinen tehtävä on hyvin avoin)
- Tekniset dokumentit (tarkkuus, laajuus, määrä)
- Ryhmätyöskentelyn dokumentointi:
- Osallistumisen seuranta
- Yhteistyön onnistuminen
- Muutostenhallinta
- Virheiden käsittely
- Perustellut koodauskielivalinnat (c#, python, ...)

## Serverit ja clientti
- dokumentaatio
- protokollat/viestit servereille ja takaisin
- koodin selkeys ja rakenne
- tietokannat

## Käyttöliittymä
- Tuotoksen esittely
- Ulkonäkö
- Toimivuus
- Käytettävyys

# ALOITA NÄISTÄ:

## Timeserver and client startup code (HTTP/JSON)
----------------------------------------------
Build me a simple HTTP/JSON server that returns current time when requested.
Build also a client that requests current time and for starters prints out the response it gets.


## Weather server
--------------
Build me a simple HTTP/JSON server that responds with
a temperature in Celsius, the amount of sunlight in % and wind speed m/s
at a given time/date.
Also build a simple client that requests the data and prints it out.


## Electricity user residence client
---------------------------------
A recidence has:
  a heating method (wood/oil furnace, heat pump, direct electricity)
  an energy usage profile that depends on external temperature and insulation
  some possible power sources  (electrical connection, solar panels)
  an energy storage (battery backup 0 - 100 kWh)
  a number of persons that use energy (make food, heat water and shower)
  a car (petrol, plug in hybrid, full electric)
  an electricity contract with transfer fees (per month plus per kWh) and usage fees (per month, per kWh plus fixed comission per kWh)
  a recidense can buy and sell electricity
We need:
  a monthly, weekly and hourly reports and charts of usage and costs
  a current centralized display of energy flow to and from energy devices (sources, sinks and storage)


## A residence has electical appliances
------------------------------------
id
name
max power

## Each appliance has a usage profile

## Aurinkopaneeli

Kysy googlelta: "miten lasken aurinkopaneelin maksimitehon asennuskulmien ja kellonajan mukaan tietyllä paikkakunnalla"

 

Selitys on aika matemaattinen.... Voit aluksi käyttää paneelin maksimitehoa tai esim 50% maksimitehosta. Hae tietoa paneeleista "aurinkovoimala omakotitaloon" -haulla.
----------------------------------
id
dateFrom
dateTo
timeFrom
timeTo
power
