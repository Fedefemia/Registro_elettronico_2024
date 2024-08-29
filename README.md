**Registro Elettronico**
Descrizione

Questo progetto è un'applicazione Windows Forms sviluppata in C# utilizzando il framework .NET. L'applicazione serve come registro elettronico per gestire le informazioni degli studenti e i loro voti. Utilizza due file principali per la memorizzazione e gestione dei dati:

    studenti.json: Contiene le informazioni sugli studenti.
    Users.txt: Contiene le credenziali degli amministratori e degli insegnanti.

Struttura dei File
studenti.json

Il file studenti.json è un file JSON che memorizza le informazioni sugli studenti. La struttura del file è la seguente:

```json
[
  {
    "matricola": 513272,
    "nome": "Aurora",
    "cognome": "Rossi",
    "data_nascita": "2008-11-22T23:00:00Z",
    "luogo_nascita": "Vicenza",
    "classe": "4CII",
    "voti": {
      "Italiano": [
        5,
        2,
        2,
        3
      ],
      "Storia": [
        8,
        2,
        3,
        2
      ],
      "Matematica": [
        4,
        4,
        7,
        2
      ],
      "Inglese": [
        4,
        8,
        3,
        8
      ],
      "Informatica": [
        6,
        4,
        2,
        4
      ],
      "Sistemi e Reti": [
        8,
        5,
        5,
        2
      ],
      "Tpsit": [
        8,
        5,
        9,
        4
      ],
      "Telecomunicazioni": [
        7,
        2,
        8,
        6
      ]
    }
  },
  ...
]
```

Campi:

    matricola: Identificativo univoco dello studente.
    nome: Nome dello studente.
    cognome: Cognome dello studente.
    data_di_nascita: Data di nascita dello studente in formato YYYY-MM-DD.
    classe: Classe frequentata dallo studente.
    voti: Materie e corrispettivi voti dello studente.

Users.txt

Il file Users.txt contiene le credenziali per accedere al sistema. Ogni riga rappresenta un utente con il seguente formato:

username;password

Esempio di contenuto:
```
insegnante1;password1
insegnante2;password2
```
Campi:

    username: Nome utente per il login.
    password: Password per il login.

Requisiti

    .NET Framework 4.7.2 (o superiore)
    Visual Studio 2019 (o superiore)

**Installazione e Configurazione**

Clonare Il repository:
`git clone https://github.com/Fedefemia/registro_elettronico_2024`
`cd registro_elettronico_2024`

Aprire il progetto in Visual Studio:

Avvia Visual Studio e apri il file .sln del progetto.

Compilare e Eseguire:

Compila e avvia l'applicazione direttamente da Visual Studio premendo F5.

**Utilizzo**

Accesso al Sistema:

All'avvio dell'applicazione, verrà presentata una schermata di login. Inserisci le credenziali fornite nel file Users.txt o la combinazione di studentenome.studentecognome e studente.matricola per accedere al sistema.

Esempio: 

```
nome utente                  password
studentenome.studentecognome studentematricola
```

Gestione degli Studenti:

Una volta effettuato l'accesso, puoi visualizzare le informazioni sugli studenti e modificare i voti e le materie tramite l'interfaccia grafica dell'applicazione. Le informazioni sugli studenti vengono caricate dal file studenti.json.
