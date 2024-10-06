[SK]
1. Štruktúra projektu
Projekt File Statistics Filter pozostáva z troch hlavných častí:

FileStatisticsFilter.CommonLibrary: Class Library, ktorá obsahuje dve hlavné triedy – SearchedFile a SearchedFiles. Tieto triedy poskytujú štruktúru pre reprezentáciu vyhľadaných súborov a ich metadát.

SearchedFile obsahuje vlastnosti ako názov súboru, veľkosť, dátum vytvorenia/modifikácie, príznak iba na čítanie atď. Poskytuje metódy na export metadát do CSV formátu.
SearchedFiles slúži ako kolekcia objektov typu SearchedFile, pričom umožňuje načítanie a uloženie súborov do CSV formátu.
FileStatisticsFilter.SearchConsoleApp: Konzolová aplikácia, ktorá umožňuje vyhľadávať súbory na základe vstupných parametrov zadaných cez príkazový riadok. Aplikácia podporuje rekurzívne prehľadávanie priečinkov a výsledky ukladá do CSV súboru.

FileStatisticsFilter.WpfApp: Aplikácia s grafickým rozhraním (WPF), ktorá umožňuje načítanie CSV súborov vygenerovaných konzolovou aplikáciou a zobrazenie štatistík o súboroch. Umožňuje filtrovanie súborov podľa priečinka a zobrazovanie detailných štatistík ako počet súborov, veľkosť, a časy vytvorenia/modifikácie.

2. Funkčnosť
FileStatisticsFilter.CommonLibrary
Trieda SearchedFile predstavuje metadáta jedného súboru a poskytuje základné vlastnosti súboru ako cesta k priečinku, názov, prípona, veľkosť a dátumy. Okrem toho obsahuje metódy na generovanie riadkov pre CSV export.
Trieda SearchedFiles reprezentuje kolekciu súborov a poskytuje možnosti načítania zo súborov CSV alebo ich uloženie do CSV. To umožňuje jednoduchú prácu so zoznamami súborov a ich štatistikami.
FileStatisticsFilter.SearchConsoleApp
Konzolová aplikácia poskytuje jednoduché rozhranie pre vyhľadávanie súborov. Používateľ zadáva vstupný priečinok, výstupný CSV súbor a voliteľne môže určiť, či sa má prehľadávať rekurzívne.
Aplikácia spracováva vstupné argumenty a generuje CSV súbor s metadátami o nájdených súboroch.
V prípade chyby (napr. ak priečinok neexistuje) aplikácia vypíše chybovú hlášku.
FileStatisticsFilter.WpfApp
Grafická aplikácia umožňuje používateľovi načítať CSV súbor, ktorý bol vygenerovaný konzolovou aplikáciou, a zobraziť podrobné štatistiky o vyhľadaných súboroch.
Používateľ môže filtrovať súbory podľa priečinka a zvoliť, či chce zahrnúť aj podpriečinky.
Zobrazené štatistiky zahŕňajú celkový počet súborov, veľkosť, najstaršie a najnovšie dátumy vytvorenia/modifikácie a počet súborov iba na čítanie.
3. Príklady použitia
Konzolová aplikácia: Používateľ spustí aplikáciu cez príkazový riadok a zadá priečinok, v ktorom sa majú vyhľadať súbory.

WPF aplikácia: Používateľ načíta windows.csv a zobrazí štatistiky o súboroch v rámci zvoleného priečinka.

4. Technické riešenia
Načítavanie a spracovanie súborov: Trieda SearchedFiles je navrhnutá na efektívne spracovanie zoznamu súborov, či už cez FileInfo alebo CSV súbory. Načítané údaje môžu byť uložené do CSV, čo umožňuje jednoduchý export a opätovné načítanie dát.
Zobrazovanie štatistík: WPF aplikácia využíva komponenty ako ListView a ComboBox na zobrazovanie a filtrovanie štatistík. Umožňuje používateľovi interaktívne získať podrobnosti o súboroch v rôznych priečinkoch.

5. Budúce vylepšenia
Rozšírené možnosti filtrovania: Pridanie pokročilejších filtrov pre typy súborov alebo veľkosti súborov by zvýšilo užitočnosť aplikácie.
Viacformátový export: Okrem CSV by mohol byť pridaný export do formátov ako XML alebo JSON.
Optimalizácia pre veľké súbory: V prípade veľmi veľkých súborov by mohla byť zlepšená efektivita načítavania a spracovania.
