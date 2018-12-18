# LedControl
Windows- ja Linux-ohjelmointi - Harjoitustyö

Harjoitustyön aiheena on toteuttaa Raspberry Pi -korttitietokoneella "liikennevalot". Raspberryyn yhdistetään kolme lediä, joiden tilaa ohjataan GPIO-pinnien kautta. 

Raspberryssä on linux-käyttöjärjestelmä, jonne tehdään sovellus, joka ohjaa GPIO-pinnien tilaa. Sovellukseen tehdään myös rajapinta, jonka kautta sovellusta voidaan ohjata sekä saadaan tietoa ledien tilasta.

Windows-käyttöjärjestelmään tehdään WPF-käyttöliittymäsovellus, jossa näkyy ledien tila ja niiden tilaa voidaan vaihtaa. WPF-sovellus kommunikoi Raspberryssä olevan sovelluksen kanssa ja voi sitä kautta muokata ledien tilaa sekä saa tiedon ledien tilasta. Sovellukseen toteutetaan myös tietokanta, jonne voidaan tallentaa ledien ohjaussekvenssejä. Ohjaussekvenssi sytyttää ja sammuttaa ledejä annetussa järjestyksessä sekä taajuudella.

Arviointikriteerit

1: WPF-sovellus. UI suunniteltu, sovellus reagoi käyttäjän toimiin (=koodia kiinnitetty UI-komponenttien tapahtumiin). Ohjedokumentti, jossa on kuvattu koko järjestelmän osat ja toiminnot. Harjotustyö on esitelty opettajalle ja itsearviointi on tehty.

2: Raspberryssä ohjelma, joka ohjaa ledejä päälle ja pois sekä antaa ledien tilatiedot

3: Ledien tilan kysely ja ohjaus WPF-sovelluksesta (WPF-sovellus ja Raspberryssä oleva sovellus kommunikoivat keskenään)

4: Tietokantaan voi tallentaa sekvenssin ledien tilan asettamiseen. Sekvenssin voi ladata tietokannasta ja Raspberry ohjaa ledejä sekvenssin mukaisesti

5: Valmis kokonaisuus. Raspberryssä oleva sovellus lähtee automaattisesti päälle Raspberryn käynnistyessä, WPF-sovellus osaa kommunikoida Raspberryn kanssa ilman, että käyttäjän tarvitsee asettaa ip-soitetta. Dokumentaatio Raspberryyn tehdyista toimenpiteistä sekä WPF-sovelluksen ja Raspberryn välisestä kommunikaatiosta.

## Projekti

Tämä projekti sisältää Windows WPF-sovelluksen. Raspberry Pi:ssä oleva sovellus on melkein sellaisenaan opettajan linkittämästä lähteestä:
https://jeremylindsayni.wordpress.com/2017/05/01/controlling-gpio-pins-using-a-net-core-2-webapi-on-a-raspberry-pi-using-windows-10-or-ubuntu/

![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/20181217__DSF6741.jpg)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/20181217__DSF6742.jpg)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/20181217__DSF6743.jpg)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/20181217__DSF6746.jpg)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/Main.png)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/SQLite_Database.png)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/SQLite_Tables.png)
![alt text](https://ilkkarytkonen.fi//images/koulu/Windows-ja-Linux-ohjelmointi/Sekvenssi.png)


YouTube-video: [linkname](https://www.youtube.com/watch?v=vh1WVexAgYE
