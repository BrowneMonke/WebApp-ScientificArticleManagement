let updateBtn = document.getElementById("updateBtn");

updateBtn.addEventListener("click", updateArticle);

function updateArticle() {
    let articleIdSpan = document.getElementById("articleIdSpan");
    let categorySelectionElem = document.getElementById("categorySelectionElem");
    
    changeArticleProperties(eval(articleIdSpan.innerText), categorySelectionElem.selectedIndex);
}

function changeArticleProperties(id, category) {
    console.log(`art_ID: ${id} ; cat: ${category}`);
    fetch(`/api/ScientificArticles/`, {
        method: 'PUT',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json'},
        body: JSON.stringify({ "id": id, "category": category })
    })
        .then(response => {
            if (!response.ok) {
                console.log("response NOT ok!");
                return response.text().then(errorText => {
                    throw new Error(`HTTP Error ${response.status}: ${errorText}`);
                });
            }
            else{
                alert("Article updated successfully!");
                return response.json();
            }
        })
        .catch(err => {
            console.log(`Error status ${err.status}: ${err.message}`);
        });
}