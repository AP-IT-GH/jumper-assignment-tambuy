Anton Doms en Jitse Tambuyzer
# Jumper

https://ap.cloud.panopto.eu/Panopto/Pages/Viewer.aspx?id=2f479b70-74d9-4e75-bec1-b2cc016b7d76

Het Jumper-project is een eenvoudige Unity ML-Agents omgeving waarin een agent moet leren meerdere obstakels te ontwijken door op het juiste moment te springen. Deze omgeving is ontworpen om het vermogen van de agent te testen om beslissingen te nemen onder druk en op basis van waarnemingen uit de omgeving.

![alt text](image.png)

## Omgevingsbeschrijving

**Opzet:**  
Een lineaire obstakel-ontwijkingstaak waarin de agent moet springen over een **continue stroom van bewegende obstakels**.

**Doel:**  
De agent moet correct springen om de inkomende obstakels te ontwijken en zo lang mogelijk overleven.

**Agents:**  
De omgeving bevat één agent.

**Beloningsstructuur:**

- **+1.0** voor succesvol ontwijken van een obstakel.
    
- **-1.0** bij falen (botsen tegen een obstakel).
    
- **-0.5** wanneer de agent te hoog springt (boven ingestelde hoogte).

## Gedragsparameters

- **Vector Observation space:**  
    3 observaties:
    
    - De verticale positie (y-as) van de agent.
        
    - De horizontale positie (x-as) van het dichtstbijzijnde obstakel.
        
    - De snelheid van de obstakels.
        
- **Acties:**  
    1 discrete actie-tak met 2 mogelijke acties:
    
    - 0: Niets doen.
        
    - 1: Springen.
        
- **Visuele Observaties:**  
    Geen.

## Float Properties: Drie

- **obstacle_speed:**  
    De snelheid waarmee de obstakels naar de agent toe bewegen.
    
    - **Standaardwaarde:** Willekeurig tussen 2 en 5 (bij start van elke episode).
        
    - **Minimum:** 2
        
    - **Maximum:** 5
        
- **jump_force:**  
    De kracht waarmee de agent omhoog springt.
    
    - **Standaardwaarde:** 5
        
    - **Minimum:** 3
        
    - **Maximum:** 7
        
- **spawn_interval:**  
    
    - **Standaardwaarde:** 2
        
    - **Minimum:** 1
        
    - **Maximum:** 3
        

## Benchmark Mean Reward

- **Benchmark Mean Reward:** 0.85

