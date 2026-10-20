# TARpe25Programmeerimine_II

## Raamatukogu

**Milline on DbContext-i ülesanne?**
DbContext on sild rakenduse ja andmebaasi vahel. See hoiab ühendust, tunneb tabelid (DbSet), jälgib muudatusi ja salvestab need andmebaasi (SaveChanges).

**Miks ei ole hea panna kõiki päringuid kasutajaliidese koodi?**
Siis läheb kood segaseks. Seda on raske lugeda, testida ja muuta. Sama päring on mitmes kohas. Kui andmebaas muutub, tuleb muuta ka liidest. Eraldi kiht on lihtsam ja korduvkasutatav.

**Millisesse kihti paigutaksid andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris?**
MVC-s paneksin selle Model kihti (teenused ja DbContext). Controller kutsub teenust ja annab andmed View'le. VIPER-is paneksin selle Interactor'i alla. Põhjus: View ja Controller/Presenter ei pea andmebaasist midagi teadma.

**Mida tuleb kontrollida enne raamatu kustutamist?**
Tuleb vaadata, kas raamatul on laenutusi (tabel Loans). Kui on, siis ei tohi kustutada, sest see rikub seose ja kaotab ajaloo. Tuleb vaadata ka, kas raamat on praegu laenutatud (ReturnDate on null). Siis tuleb see enne tagastada.
