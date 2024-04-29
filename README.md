# Kandidaattiprojekti - Tarinatalo minipelit

## Pelien sanojen muokkaus
### Kirjahylly
Kirjahyllypelissä on 3 tasoa, joissa jokaisessa on 3 hyllyä täytettävänä. Jokaiselle hyllylle vaaditaan 3 saman kategorian kirjaa/sanaa. Jokaisessa tasossa on myös ylimääräisiä kirjoja, jotka ovat hämäyksenä. Tällä hetkellä hämäyskirjoja on 4 kappaletta per pelin taso. Yhdelle pelin tasolle täytyy asettaa yhdeksän kirjaa kolmella eri kategoria-arvolla hämäyskirjojen lisäksi.

Sanat haetaan json-tiedostosta jonka sijainti on `Assets/Kirjahylly/Resources/words_data.json`.

Json-tiedostossa on jokaiselle kolmelle pelin tasolle oma root objektin key:
```json
{
	"bookset1": [...],
	"bookset2": [...],
	"bookset3": [...]
}
```

Näissä arvona on lista objekteja joista luodaan kirja.
```json
{
	"word": "juosta", // str: sana kirjalle
	"category": 1 // int: kategorian numero
},
```

Yhteen sopivat sanat yhdistetään logiikassa vertailemalla kategorian numeroa. Hämäyssanoilla kategorian numeroksi on annettu luku, jota ei ole millään muulla sanalla/kirjalla.


### Purkit
Hillopurkkien synonyymipankki löytyy sijainnista `Assets/Hillopurkit/SynonoymResources`.
Synonyymijoukot ovat lajiteltu kolmeen txt-tiedostoon sanaluokan perusteella - adjektiiveihin, substantiiveihin ja verbeihin.

Esimerkki txt-tiedoston synonyymijoukosta:
```
puhua|turinoida|hölöttää|pälättää|löpistä|tarinoida|turista
```

Pelilogiikka valitsee kullekin kierrokselle satunnaisesti yhden oikean synonyymijoukon ja yhden väärän sanan samasta sanaluokasta.
Kierrosten aikana aiemmin esiintyneet oikeat synonyymijoukot eivät toistu.

Synonyymijoukkoja on sanapankissa ennestään 30 kpl (10 kutakin sanaluokkaa) ja niiden laatu on varmistettu.
Yhden purkkipelin aikana niistä käytetään enintään 10:tä, joten kierrosten vaihtelevuus riittää useaan uudelleenpeluuseen.

Mikäli synonyymijoukkoa halutaan muokata huomioikaa seuraavat seikat.
1) Syntaksi \
	&emsp; a) Erota sanat pystyviivalla: sana_1|sana_2|...|sana_n-1|sana_n. \
    	&emsp; b) Yksi sanajoukko per rivi. \
	&emsp; c) Ei tyhjiä rivejä.
2) Vähintään 7 sanaa per synonyymijoukko.
3) Vältä risteävien merkitysten lisäämistä (Esimerkiksi sanan 'kulta' voi ymmärtää sekä sanojen 'raha' että 'rakas' synonyymiksi).
4) Pitkät sanat tarvitsevat väliviivan asettuakseen kahdelle riville. \
 	&emsp; Sana 'raakalainen' mahtuu yhdelle riville, mutta 'suussasulava' ei. Siksi tiedostossa sana on muodossa 'suussa-sulava'. \
	&emsp; Myös välilyönti jakaa sanat kahdelle riville, esim. 'vetää lonkkaa'. 


### Arkku
haskdfhalkfj
