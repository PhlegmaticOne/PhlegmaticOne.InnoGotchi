$(() => {
    const petCards = document.querySelectorAll('.pet-card');
    petCards.forEach(card => {
        console.log(card.innerHTML);
    });
});