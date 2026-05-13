# 📦 ERP - Controllo Scorte

Modulo ASP.NET Core per il **Controllo Scorte** del sistema ERP.

## Come avviare

1. Modifica la connection string in `appsettings.json` con il database del team
2. Esegui `dotnet run`
3. Apri http://localhost:5000 per la documentazione Swagger

## API disponibili

| Metodo | Endpoint | Cosa fa |
|--------|----------|---------|
| GET | `/api/prodotti` | Lista prodotti |
| GET | `/api/prodotti/{id}` | Dettaglio prodotto |
| POST | `/api/prodotti` | Crea prodotto |
| PUT | `/api/prodotti/{id}` | Modifica prodotto |
| DELETE | `/api/prodotti/{id}` | Elimina prodotto |
| GET | `/api/movimenti` | Lista movimenti |
| POST | `/api/movimenti` | Registra carico/scarico |
| GET | `/api/alert` | Alert scorte basse |
| PUT | `/api/alert/{id}/risolvi` | Risolvi alert |
| POST | `/api/alert/verifica` | Controlla tutte le scorte |
| GET | `/api/dashboard` | Riepilogo magazzino |

## Esempio: registrare uno scarico

```json
POST /api/movimenti
{
  "prodottoId": 1,
  "tipo": "Scarico",
  "quantita": 200,
  "causale": "Ordine cliente #123",
  "operatore": "Mario Rossi"
}
```
