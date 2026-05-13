// Chiude gli alert dopo 5 secondi
document.addEventListener('DOMContentLoaded', function() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Conferma per le azioni pericolose
document.addEventListener('DOMContentLoaded', function() {
    const deleteButtons = document.querySelectorAll('a[href*="/Delete"]');
    deleteButtons.forEach(btn => {
        btn.addEventListener('click', function(e) {
            if (!confirm('Sei sicuro di voler continuare? Questa azione non può essere annullata.')) {
                e.preventDefault();
            }
        });
    });
});
