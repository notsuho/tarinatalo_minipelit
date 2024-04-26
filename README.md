# Kandidaattiprojekti - Tarinatalo minipelit

### Pelien sanojen muokkaus
#### Kirjahylly
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

#### Purkit
#### Arkku