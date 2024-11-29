# Inhalt der Projektmappe

> - CreateSearchablePDF2: [Description](https://github.com/AgileObject/P_KI_OCRDemo_UI/blob/master/CreateSearchablePDF2/README.md)
> - KI_OCRDemo_UI


# KI_OCRDemo_UI - KODO

***Anmerkung:*** Ein Großteil dieser Dokumentation wurde mit GitHub CoPilot generiert.

#   

## Ziel

Ziel dieser Anwendung ist
1. Das Zeigen von Ergebnissen verschiedener OCR's aus Azure und AWS
2. Das Zeigen von durch OpenAI (ChatGPT) verbesserten OCR-Ergebnissen
3. Das Zeigen von OCR-Ergebnissen, die direkt aus ChatGPT (gpt-4o) generiert wurden.
4. Ein Gefühl für die Performance der Aufrufe zu vermitteln
5. Grundsätzlich: Eine Vergleichbarkeit herstellen...

Kein Ziel war...
1. Schöne Oberflächen zu bauen
2. Perfeketen Clean Code zu erzeugen
3. Unit Testing bereit zu stellen
4. Aktuellste Frameworks einzubinden und zu verwenden

#   

## Beschreibung

Dieser Code implementiert eine Windows Forms-Anwendung, die OCR (Optical Character Recognition) auf ausgewählten Bilddateien durchführt und die Ergebnisse anzeigt. Hier sind die Hauptfunktionen des Codes:
1.	Dateiauswahl:

  > •	Der Benutzer kann eine Bilddatei auswählen, die dann in einer PictureBox angezeigt wird.

2.	OCR-Verarbeitung:
  
  > •	Der Benutzer kann zwischen zwei OCR-Diensten wählen: Azure Computer Vision und AWS Textract.

  > •	Je nach Auswahl wird die OCR auf der ausgewählten Bilddatei durchgeführt und das Ergebnis in einem Textfeld angezeigt.

4.	Integration mit OpenAI GPT:

  > •	Der Benutzer kann das OCR-Ergebnis mit GPT optimieren lassen.

  > •	Der Benutzer kann auch direkt eine Bildanalyse mit GPT durchführen, indem er einen Prompt eingibt.

5.	Initialisierung:
   
  > •	Die Anwendung liest verschiedene Konfigurationsparameter und Geheimnisse (z.B. API-Schlüssel) aus und initialisiert die entsprechenden Dienste.

6.	Logging:
  
  > •	Die Anwendung verwendet log4net für das Logging von Informationen und Fehlern.


Hier ist eine kurze Übersicht der wichtigsten Methoden und deren Funktionen:

  > •	buttonSelectFile_Click: Öffnet einen Dateiauswahldialog und zeigt die ausgewählte Bilddatei an.

  > •	buttonDoOCR_Click: Führt die OCR-Verarbeitung mit dem ausgewählten Dienst durch und zeigt das Ergebnis an.

  > •	buttonOptimizeGPT_Click: Optimiert den OCR-Text mit GPT.

  > •	buttonDirectReadingGPT_Click: Führt eine direkte Bildanalyse mit GPT durch und zeigt das Ergebnis an.

  > •	InitializeAppl: Initialisiert die Anwendung und lädt Konfigurationsparameter und Geheimnisse.

Die Anwendung verwendet verschiedene Bibliotheken und Dienste, darunter Azure Computer Vision, AWS Textract und OpenAI GPT, um die OCR- und Textverarbeitungsfunktionen zu implementieren.

#   

## Requirements

  > - Visual Studio 2022
  > - .NET 8.0
  > - Zugriff auf die OpenAI API (User API-Key erforderlich)
  > - Zugriff auf AWS - AmazonTextractFullAccess
  > - Zugriff auf Microsoft Azure  

#   

## Preparation

### OpenAI

  > - User Account erforderlich
  > - OpenAI Key erforderlich
  ![image](https://github.com/user-attachments/assets/223cec22-d461-4b2d-b1dc-8b4b1a247b57)

#   

### AWS

  > - AWS Zugriff (User Account - im IAM angelegter Benutzer mit entsprechenden Berechtigungen)
  ![image](https://github.com/user-attachments/assets/33cf9630-9fde-493c-9ab8-196dadb38792)
  > - Für den Zugriff auf AWS ist die Berechtigung auf AmazonTextract... erforderlich
  ![image](https://github.com/user-attachments/assets/6f19d372-b36a-4cb7-8b19-3466fd30cfc1)

#   

### Azure

  > - Azure Zugriff (User Account mit entsprechenden Berechtigungen)
  > - Anlage einer Ressourcengruppe
  ![image](https://github.com/user-attachments/assets/a47bf1ce-07df-45c3-b152-527a9a2ca371)
  > - Anlage einer Instanz für die Verwendung der Azure KI Vision
  ![image](https://github.com/user-attachments/assets/81f0b4ad-25db-4ff5-9cbb-907f38b2e9a5)

#   
 
### Konfiguration

***Konfiguration der oben beschriebenen Schlüssel und Endpunkte in der app.config***

![image](https://github.com/user-attachments/assets/b39c070e-edac-4bdc-84cc-e7efc970e135)

***log4net.config***

![image](https://github.com/user-attachments/assets/6a4def1c-943f-4f22-ae31-e121e9538563)

#   

## Projektaufbau

![image](https://github.com/user-attachments/assets/4a0c2809-4735-4835-aaa2-ab51d60c67eb)

Alle Funktionalitäten sind in Klassen für sich komplett gekapselt und wiederverwendbar.

#    

### Secrets

Die Zugriffe der Anwendung sind in einer applicationsspezifischen Konfigurationsdatei hinterlegt. Alternativ können die Parameter für die Anwendung als Umgebungsvariablen gesetzt werden. Die Klassen lassen zudem eine Übergabe als Param zu.

![image](https://github.com/user-attachments/assets/9dce5f75-7992-436c-86c0-20f2d73f2203)

Exemplarisch wird hier die Zugriffsklasse ***AWSSecrets.cs*** beschrieben...

![image](https://github.com/user-attachments/assets/99d28c0d-8262-41fa-953b-8cd0aa3407ee)

Die Klasse AWSSecrets im Namensraum KI_OCRDemo_UI.AWS_Textract_Secrets dient dazu, die AWS-Zugangsdaten (Access Key ID und Secret Access Key) zu laden und zu verwalten. Diese Zugangsdaten werden benötigt, um auf AWS-Dienste wie Textract zuzugreifen. Hier sind die Hauptfunktionen der Klasse:

1.	Deklarationen:

  > •	ILog _Logger: Ein Logger-Objekt für das Logging von Informationen und Fehlern.

  > •	EnvironmentVariableKey und EnvironmentVariableEndpoint: Namen der Umgebungsvariablen, die die AWS-Zugangsdaten enthalten können.

  > •	AccessKeyId und SecretAccessKey: Statische Eigenschaften, die die geladenen AWS-Zugangsdaten speichern.

2.	LoadSucceeded:
   
  > •	Diese Methode versucht, die AWS-Zugangsdaten zu laden, indem sie nacheinander verschiedene Quellen überprüft: Kommandozeilenargumente, Umgebungsvariablen und die App-Konfigurationsdatei.

  > •	Wenn das Laden erfolgreich ist, werden die letzten Zeichen der Zugangsdaten maskiert und geloggt.

  > •	Die Methode gibt true zurück, wenn beide Zugangsdaten erfolgreich geladen wurden, andernfalls false.

3.	LoadKeySucceeded:

  > •	Diese Methode versucht, die Access Key ID zu laden.

  > •	Sie überprüft zuerst die Kommandozeilenargumente, dann die Umgebungsvariablen und schließlich die App-Konfigurationsdatei.

  > •	Die Methode gibt true zurück, wenn die Access Key ID erfolgreich geladen wurde, andernfalls false.
4.	LoadEndpointSucceeded:

  > •	Diese Methode versucht, den Secret Access Key zu laden.

  > •	Sie überprüft zuerst die Kommandozeilenargumente, dann die Umgebungsvariablen und schließlich die App-Konfigurationsdatei.

  > •	Die Methode gibt true zurück, wenn der Secret Access Key erfolgreich geladen wurde, andernfalls false.

5.	IsValidKey:

  > •	Diese Methode überprüft, ob ein gegebener Schlüssel (key) das richtige Format hat.

  > •	Ein gültiger Schlüssel muss ein 32-stelliges Hexadezimal-Zeichen sein.

  > •	Die Methode gibt true zurück, wenn der Schlüssel gültig ist, andernfalls false.

Zusammengefasst, stellt die Klasse AWSSecrets sicher, dass die notwendigen AWS-Zugangsdaten aus verschiedenen Quellen geladen und validiert werden, bevor sie in der Anwendung verwendet werden.

Die Zugriffsklassen für Azure und OpenAI funktionieren analog. Deshalb lasse ich die explizite Beschreibung hier weg...

#   

### OCR

Die aktuell in diesem Projekt verwendeten OCR liegen hier: 

![image](https://github.com/user-attachments/assets/c6d0071d-a482-4d72-9aae-5d73317aac9b)

***Anmerkung:*** Für beide OCR's wurde keine Optimierung vorgenommen. Ich bin mir sicher, dass in beiden Varianten mehr an Ergebnis möglich ist!

#   

#### AWS Textract

Der Code definiert eine Klasse textractOCR im Namensraum KI_OCRDemo_UI.OCR, die die AWS Textract API verwendet, um OCR (Optical Character Recognition) auf einer gegebenen Bilddatei durchzuführen.

![image](https://github.com/user-attachments/assets/423cdbf7-0d91-404d-924c-69451a7c5970)

Hier sind die Hauptfunktionen der Klasse:

1.	Deklarationen:
   
  > •	ILog _Logger: Ein Logger-Objekt für das Logging von Informationen und Fehlern.

2.	GetOCRResultFromFile:

  > •	Diese Methode nimmt den Dateinamen einer Bilddatei als Eingabe und führt OCR darauf durch.

  > •	Es wird ein Zeitstempel generiert, um den Namen der Ergebnisdatei zu erstellen.

  > •	Die Bilddatei wird gelesen und in einen MemoryStream geladen.

  > •	Ein AmazonTextractClient wird mit den AWS-Zugangsdaten initialisiert.

  > •	Ein AnalyzeDocumentRequest wird erstellt, um die Bilddatei zu analysieren. Es werden die Feature-Typen FORMS verwendet.

  > •	Die Analyse wird asynchron durchgeführt, und die Antwort wird verarbeitet, um die Textblöcke zu extrahieren.

  > •	Die Methode erstellt Key-Value-Mappings aus den extrahierten Blöcken und schreibt diese in die Ergebnisdatei.

  > •	Die Methode gibt den Namen der Ergebnisdatei zurück.

3.	Get_kv_relationship:

  > •	Diese Methode nimmt Listen von Schlüssel- und Wertblöcken sowie eine Blockkarte und erstellt ein Dictionary, das die Schlüssel-Wert-Beziehungen enthält.

  > •	Sie findet die entsprechenden Wertblöcke für jeden Schlüsselblock und extrahiert den Text aus beiden, um die Beziehungen zu erstellen.

4.	Find_value_block:

  > •	Diese Methode findet den entsprechenden Wertblock für einen gegebenen Schlüsselblock, indem sie die Beziehungen des Schlüsselblocks durchsucht.

5.	Get_text:

  > •	Diese Methode extrahiert den Text aus einem gegebenen Block und seinen untergeordneten Blöcken.

  > •	Sie durchsucht die Beziehungen des Blocks und fügt den Text der untergeordneten Blöcke zusammen.

Zusammengefasst, verwendet die Klasse textractOCR die AWS Textract API, um Text aus einer Bilddatei zu extrahieren und die extrahierten Schlüssel-Wert-Paare in einer Textdatei zu speichern.

#   

#### Azure KI Vision

![image](https://github.com/user-attachments/assets/f15a6b06-2ae9-4b48-afb8-38afa2f30b20)

Der Code definiert eine Klasse azureComputerVision im Namensraum KI_OCRDemo_UI.OCR, die die Azure Computer Vision API verwendet, um eine Bilddatei zu analysieren und die Ergebnisse in einer Textdatei zu speichern. Hier sind die Hauptfunktionen der Klasse:

1.	Deklarationen:

  > •	ILog _Logger: Ein Logger-Objekt für das Logging von Informationen und Fehlern.

2.	ImageAnalysisSample_Analyze_File:

  > •	Diese Methode nimmt den Dateinamen einer Bilddatei als Eingabe und führt eine Bildanalyse darauf durch.

  > •	Es wird ein Zeitstempel generiert, um den Namen der Ergebnisdatei zu erstellen.

  > •	Die Bilddatei wird gelesen und als VisionSource initialisiert.

  > •	Ein VisionServiceOptions-Objekt wird mit den Azure-Zugangsdaten initialisiert.

  > •	Ein ImageAnalysisOptions-Objekt wird erstellt, um die gewünschten Analyse-Features festzulegen (z.B. Objekte, Personen, Text, Tags).

  > •	Ein ImageAnalyzer wird mit den Service- und Bildquellenoptionen initialisiert.

  > •	Die Analyse wird synchron durchgeführt, und die Antwort wird verarbeitet, um die Ergebnisse zu extrahieren.

  > •	Die Ergebnisse der Analyse (z.B. Bildhöhe, Bildbreite, erkannte Objekte, Text) werden in eine Textdatei geschrieben.

  > •	Zusätzlich wird eine separate Textdatei erstellt, die nur die OCR-Textresultate enthält.

  > •	Die Methode gibt den Namen der Datei mit den OCR-Textresultaten zurück.

3.	Ergebnisverarbeitung:

  > •	Die Methode verarbeitet verschiedene Analyseergebnisse wie Bildgröße, erkannte Objekte, Personen, Tags und Text.

  > •	Die Ergebnisse werden in eine Textdatei geschrieben.

  > •	Die JSON-Antwort der Analyse wird ebenfalls verarbeitet, um nur die Textresultate zu extrahieren und in eine separate Datei zu schreiben.

4.	Fehlerbehandlung:
  
  > •	Wenn die Analyse fehlschlägt, werden die Fehlerdetails in die Ergebnisdatei geschrieben.

  > •	Die Methode gibt eine Fehlermeldung zurück, wenn ein Fehler auftritt.

Zusammengefasst, verwendet die Klasse azureComputerVision die Azure Computer Vision API, um eine Bilddatei zu analysieren und die Ergebnisse in Textdateien zu speichern. Die Methode ImageAnalysisSample_Analyze_File führt die Analyse durch, verarbeitet die Ergebnisse und speichert sie in Dateien.

#   

### ChatGPT

Die Anbindung von ***ChatGPT*** erfolgt hier:

![image](https://github.com/user-attachments/assets/16f55141-c1ee-40f3-9bbe-70fe4aba815a)

![image](https://github.com/user-attachments/assets/4ecfd214-9d9c-43d7-aa35-948d3e8ef48b)


Dieser Code definiert eine Klasse ChatGptService innerhalb des Namensraums KI_OCRDemo_UI.OpenAI. Diese Klasse bietet zwei Hauptfunktionen, die mit dem OpenAI GPT-4 Modell interagieren: Textkorrektur und Bildanalyse. Hier ist eine detaillierte Beschreibung der Funktionen:

1.	Deklarationen und Konstruktor:
   
  > •	ILog _Logger: Ein Logger-Objekt für das Logging von Informationen und Fehlern.

  > •	HttpClient httpClient: Ein statisches HttpClient-Objekt für HTTP-Anfragen.

  > •	ChatGPTService(): Ein leerer Konstruktor (vermutlich ein Tippfehler, sollte ChatGptService sein).

2.	CorrectTextAsync:

  > •	Diese Methode nimmt einen Eingabetext (inputText) und sendet eine Anfrage an die OpenAI API, um den Text zu korrigieren.

  > •	Es wird ein JSON-Objekt erstellt, das das Modell (gpt-4o), die Rollen (system und user) und den zu korrigierenden Text enthält.

  > •	Die Anfrage wird an die OpenAI API gesendet, und die Antwort wird verarbeitet, um den korrigierten Text zu extrahieren.

  > •	Der korrigierte Text wird zurückgegeben.

4.	ImageAnalysis:

  > •	Diese Methode nimmt den Pfad zu einem Bild (imagePath) und einen Prompt-Text (promptText) und sendet eine Anfrage an die OpenAI API, um eine Bildanalyse durchzuführen.

  > •	Das Bild wird in ein Base64-String konvertiert und zusammen mit dem Prompt-Text in einem JSON-Objekt verpackt.

  > •	Die Anfrage wird an die OpenAI API gesendet, und die Antwort wird verarbeitet, um das Ergebnis der Bildanalyse zu extrahieren.

  > •	Das Ergebnis wird zurückgegeben.

Hier ist eine kurze Übersicht der wichtigsten Methoden und deren Funktionen:

  > •	CorrectTextAsync: Korrigiert Grammatik und Rechtschreibung des Eingabetextes mithilfe des GPT-4 Modells.

  > •	ImageAnalysis: Führt eine Bildanalyse durch, indem ein Bild und ein Prompt-Text an die OpenAI API gesendet werden.

Beide Methoden verwenden HttpClient, um HTTP-POST-Anfragen an die OpenAI API zu senden, und verarbeiten die JSON-Antworten, um die gewünschten Informationen zu extrahieren.

#   

## UI

Die Oberfläche besteht aus 5 Buttons, 2 Checkboxen, drei Anzeigefenstern und einem Eingabeprompt... 

![image](https://github.com/user-attachments/assets/1312d66f-7736-49a8-b5ad-4e843bf274e1)


### Select File

![image](https://github.com/user-attachments/assets/ea3c5b3b-7621-42b0-8620-4f7ce6af936e)

![image](https://github.com/user-attachments/assets/2fe0cd58-12c7-469c-b9f8-18776c980ca3)

## do OCR

Mit Hilfe der beiden Checkboxen ![image](https://github.com/user-attachments/assets/6ddd75a2-d8ef-4d87-a2dd-c6b10a80ec59)
kann man die jeweilig einzusetzende OCR auswählen...

Der Button "do OCR" erzeugt dann mit Hilfe der ausgewählten OCR ein Ergebnis...

![image](https://github.com/user-attachments/assets/a77cc588-0273-4bcc-b174-e5d7d0f558c5)

## optimize with ChatGPT

Das OCR Ergebnis kann direkt in ChatGPT optimiert werden.

![image](https://github.com/user-attachments/assets/c1bfe3e0-fed8-4ffd-af8f-0f819cc44b44)

![image](https://github.com/user-attachments/assets/28cca15b-e2f9-44a9-a22b-af89a758e9f4)

## direct reading with ChatGPT

Es gibt auch die Variante, keine OCR zu verwenden und das Bild direkt von ChatGPT erkennen zu lassen.

![image](https://github.com/user-attachments/assets/b8676b8c-a3c7-4846-87ef-1503e14f0679)

![image](https://github.com/user-attachments/assets/4618fe72-3a91-47e5-98dc-d17362e519e0)

Aber! Nicht alle Informationen sind lesbar! Gerade Handschriften stellen hier ein Problem da!

![image](https://github.com/user-attachments/assets/09a3bd7a-9da3-4bb6-a083-612a07acc41a)

Nutzung des GPT Promps

![image](https://github.com/user-attachments/assets/084303f0-7242-4cd4-9d68-eee2f757b292)

![image](https://github.com/user-attachments/assets/c9b09726-b7ef-4c30-a714-bd8555a2e12f)

![image](https://github.com/user-attachments/assets/14047c89-9fd6-48f1-bb92-7df2e5545284)

![image](https://github.com/user-attachments/assets/3c93e549-b18b-41be-82e5-36590faf54d0)













