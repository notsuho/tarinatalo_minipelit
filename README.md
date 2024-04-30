# Kandidaattiprojekti - Tarinatalo minipelit

## Pelien sanojen muokkaus
### Kirjahylly
Kirjahyllypelissä on 3 tasoa, joissa jokaisessa on 3 hyllyä täytettävänä. Jokaiselle hyllylle vaaditaan 3 saman kategorian kirjaa/sanaa. Jokaisessa tasossa on myös ylimääräisiä kirjoja, jotka ovat hämäyksenä. Tällä hetkellä hämäyskirjoja on 4 kappaletta per pelin taso. Yhdelle pelin tasolle täytyy asettaa yhdeksän kirjaa kolmella eri kategoria-arvolla hämäyskirjojen lisäksi.

Sanat haetaan json-tiedostosta jonka sijainti on `Assets/Kirjahylly/Resources/words_data.json`.

Json-tiedostossa on array, joka sisältää objektin jokaista pelin tasoa kohden:
```json
{
    "word_groups": [
        ["kähveltää", "ryövätä", "varastaa"],
        ["nukkua", "koisia", "uinua"],
        ["huijata", "juksata", "naruttaa"]
    ],
    "filler_words": ["vaappua", "puhista", "uskaltaa", "tukehtua"]
},
```

`word_groups` on lista listoja, jotka sisältävät oikeat sanat ryhmiteltyinä. `word_groups`:in oletetaan olevan lista joka sisältää 3 listaa, ja näiden listojen oletetaan sisältävän 3 stringiä. `filler_words`:in oletetaan sisältävän 4 stringiä. Samassa listassa olevat sanat merkataan samalla kategorianumerolla BookManager.LoadBookDataFromFile() -metodissa. Tätä kategorianumeroa käytetään pelin logiikassa tarkistamaan, onko hyllyllä saman kategorian kirjoja. Täytesanoille annetaan kategorianumeroksi `null`.
Kun sanat luetaan json-tiedostosta ne sekoitetaan, jotta ne tulisivat satunnaisesssa järjestyksessä pöydälle. Myös eri "sanasetit" sekoitetaan ennen pelin aloitusta, joten ne tulevat eri järjestyksessä pelaajalle. Tämä myös mahdollistaa useamman setin lisäämisen json-tiedostoon ja kolme satunnaista sanasettiä valitaan pelattavaksi.


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

Arkku-pelissä pelaaja saa eteensä lauseen, josta puuttuu yksi sana, sekä kaksi sanavaihtoehtoa. Hänen täytyy valita, kumpi sanoista sopii lauseeseen merkitykseltään paremmin .

Lauseet ja sanat haetaan json-tiedostosta, jonka sijainti on `Assets/Arkku/Resources/exerciseMaterial.json`

Json-tiedostossa on array `exercises`, jossa on tehtävämatriaali seuraavanalaisessa muodossa:
```json
    {
        "sentence": "Lääkärin täytyi tehdä <sprite name=\"line\"> leikkaus.",
        "word1": "kiireellinen",
        "word2": "kiireinen",
        "correctAnswer": "kiireellinen",
        "explanation": "Leikkauksella oli kiire, lääkärillä ei.<br><br><mark=#77432e40>Kiireinen</mark> viittaa ihmiseen tai ajanjaksoon.<br><br><mark=#77432e40>Kiireellinen</mark> viittaa asiaan, joka täytyy hoitaa nopeasti."
    }
```

`sentence` sisältää lauseen, johon sanaa sovitetetaan. `<sprite name=\"line\">` merkitsee arvattavan sanan kohtaa eli tyhjää viivaa. `word1` ja `word2` ovat kaksi sanaa, joita arvuutellaan lauseeseen. `correctAnswer` on oikea vastaus eli lauseeseen paremmin sopiva sana. `explanation` on selitys, siitä mitä sanat tarkoittavat.

LevelManager.Start()-metodissa haetaan json-tiedostosta harjoitukset ja arvotaan niistä muuttujalla säädeltävä määrä mukaan peliin. Jsoniin voi lisätä vapaasti vastaavan sisältöistä materiaalia. 
