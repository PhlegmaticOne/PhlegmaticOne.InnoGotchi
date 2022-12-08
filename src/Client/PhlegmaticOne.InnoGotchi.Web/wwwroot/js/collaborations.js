$(() => {
    const searchResult = document.getElementById("search_result");
    document.getElementById("search_profiles_button").addEventListener("click", async () => await searchProfiles());

    async function searchProfiles() {
        const input = document.getElementById("search_profiles_input");
        const inputValue = input.value;
        input.value = "";

        if (inputValue === "") {
            return;
        }

        const response = await window.fetch(`/Search/SearchPartial?searchText=${inputValue}`);

        if (response.ok) {
            searchResult.innerHTML = await response.text();
            subscribeInviteButtons();
        }
    }

    function subscribeInviteButtons() {
        document.querySelectorAll(".profile_search_result").forEach(result => {
            const collaborateButton = result.querySelector(".invite_button");
            collaborateButton.addEventListener("click", async () => await collaborate(result));
        });
    }

    async function collaborate(profileElement) {
        const idModel = getIdModel(profileElement);

        const result = await window.fetch("/Collaborations/Collaborate",
            {
                method: "POST",
                headers: { 'Content-type': "application/json" },
                body: JSON.stringify(idModel)
            });

        if (result.ok) {
            searchResult.removeChild(profileElement);
        }
    }

    function getIdModel(profileElement) {
        const id = profileElement.querySelector(".profile_id").value;
        return {
            id: id
        };
    }
})