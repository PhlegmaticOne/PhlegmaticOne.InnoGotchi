$(() => {

    document.querySelectorAll(".pet-card").forEach(card => {
        subscribeFeedButton(card);
        subscribeDrinkButton(card);
    });

    function subscribeFeedButton(card) {
        const feedButton = card.querySelector(".feed_button");
        feedButton.addEventListener("click",
            async () => {
                await performAction("/InnoGotchies/FeedPartial", card);
            });
    }

    function subscribeDrinkButton(card) {
        const drinkButton = card.querySelector(".drink_button");
        drinkButton.addEventListener("click",
            async () => {
                await performAction("/InnoGotchies/DrinkPartial", card);
            });
    }

    async function performAction(url, card) {
        const idModel = getIdModel(card);

        const response = await window.fetch(url,
            {
                method: "POST",
                headers: { 'Content-type': "application/json" },
                body: JSON.stringify(idModel)
            });

        if (response.ok) {
            const parent = card.parentElement;
            parent.innerHTML = await response.text();

            subscribeFeedButton(parent.firstElementChild);
            subscribeDrinkButton(parent.firstElementChild);
        }
    }

    function getIdModel(card) {
        const id = card.querySelector(".pet_id").value;
        const canSeeDetails = document.getElementById("can-see-details").value === "true";
        return {
            id: id,
            canSeeDetails: canSeeDetails
        };
    }
});