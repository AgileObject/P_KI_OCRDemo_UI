# KI_OCRDemo_UI - KODO

## Beschreibung

Diese Anwendung ist in C# entwickelt. 

Ziel dieser Anwendung ist
1. Das Zeigen von Ergebnissen verschiedener OCR's aus Azure und AWS
2. Das Zeigen von durch OpenAI (ChatGPT) verbesserten OCR-Ergebnissen
3. Das Zeigen von OCR-Ergebnissen, die direkt aus ChatGPT (gpt-4o) generiert wurden.
4. Grundsätzlich: Eine Vergleichbarkeit herstellen...


## Projektaufbau

![image](https://github.com/user-attachments/assets/b5ff0274-b748-47a8-888c-11fd390ec23d)

Alle Funktionalitäten sind in Klassen für sich komplett gekapselt und wiederverwendbar.

Die Zugriffe der Anwendung sind in einer applicationsspezifischen Konfigurationsdatei hinterlegt. Alternativ können die Parameter für die Anwendung als Umgebungsvariablen gesetzt werden. Die Klassen lassen zudem eine Übergabe als Param zu.

![image](https://github.com/user-attachments/assets/89053a7a-d916-44f2-9035-791ce32dafbe)


![image](https://github.com/user-attachments/assets/4ce048d1-4f97-4400-bc7b-4281991afbdc)

Die aktuell in diesem Projekt verwendeten OCR liegen hier: 

![image](https://github.com/user-attachments/assets/c6d0071d-a482-4d72-9aae-5d73317aac9b)

*Hinweis:* Für beide OCR's wurde keine Optimierung vorgenommen. Ich bin mir sicher, dass in beiden Varianten mehr an Ergebnis möglich ist!

## UI

Die Oberfläche besteht aus 5 Buttons, 2 Checkboxen und drei Anzeigefenstern... 

![image](https://github.com/user-attachments/assets/71d0625c-ff4c-487b-a034-78bfc212d541)

### Select File

![image](https://github.com/user-attachments/assets/373df3cb-a2e2-478a-adb4-b86bd2ade33d)

![image](https://github.com/user-attachments/assets/cc77ae4e-2ec4-4c19-96ed-f15d006b33b0)

## do OCR

Mit Hilfe der beiden Checkboxen ![image](https://github.com/user-attachments/assets/6ddd75a2-d8ef-4d87-a2dd-c6b10a80ec59)
kann man die jeweilig einzusetzende OCR auswählen...

Der Button "do OCR" erzeugt dann mit Hilfe der ausgewählten OCR ein Ergebnis...

![image](https://github.com/user-attachments/assets/befa8c99-adad-4ab3-94d7-9f99f03f6291)

## optimize with ChatGPT

Das OCR Ergebnis kann direkt in ChatGPT optimiert werden.

![image](https://github.com/user-attachments/assets/1bcec65e-b4b9-466d-8d90-fd9da1cf5fef)

Es gibt auch die Variante, keine OCR zu verwenden und das Bild direkt von ChatGPT erkennen zu lassen.

![image](https://github.com/user-attachments/assets/f9bdfe80-7606-4f9a-9b6a-3e91b0a480c4)











