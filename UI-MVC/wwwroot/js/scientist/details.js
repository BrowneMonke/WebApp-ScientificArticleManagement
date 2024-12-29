const scientistId = Number(document.getElementById('scientistIdentification').innerText);
const articlesTableBody = document.querySelector('#articlesTable tbody');
const addArticleBtn = document.getElementById('addArticleBtn');

const categoryEnum = {
    0: "Astrophysics",
    1: "Neuroscience",
    2: "Marine Ecology",
    3: "Quantum Physics",
    4: "Electromagnetism",
    5: "Chemistry",
    6: "Biology"
};
const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];


if (addArticleBtn) {
    addArticleBtn.addEventListener('click', ev => {
        ev.preventDefault();
        submit();
    });
}
window.addEventListener('load', init);


function submit() {
    const articleId = document.getElementById('articlesList').value;
    const isLeadResearcher = document.getElementById('isLeadResearcherCheckbox').checked;

    if (articleId === "" || articleId == null) {
        console.log("Pressed 'Add article' when all articles already assigned!");
        alert("All articles have already been assigned!");
        resetLeadResearcherCheckbox();
        return;
    }
    
    postArticleScientistLink(articleId, isLeadResearcher);
}

function postArticleScientistLink(articleId, isLeadResearcher){
    fetch('/api/ArticleScientistLinks', {
        method: 'POST', headers: {
            'Accept': 'application/json', 'Content-Type': 'application/json'
        }, body: JSON.stringify({
            "articleId": articleId, "scientistId": scientistId, "isLeadResearcher": isLeadResearcher
        })
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(errorText => { // Get error text
                    throw new Error(`HTTP error ${response.status}: ${errorText}`);
                });
            }
            console.log("Article assigned successfully!");
            init();
        })
        .catch(error => {
            console.error("Error assigning article:", error);
            alert("Error assigning article. Check the console for details.");
        });
}

function init() {
    loadArticlesByScientist();
    loadArticleAssignmentForm();
}


function loadArticlesByScientist() {
    fetch(`/api/ScientificArticles/by-scientist/${scientistId}`, {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => showArticles(data))
        .catch(err => {
            console.log(`Something went wrong.. :-/
                ${err.message}`);
            alert(`Error! Check the console for details.`);
        });
}

function showArticles(articles) {
    articlesTableBody.innerHTML = '';
    let rowNo = 1;
    articles.forEach(art => addArticleToList(art, rowNo++));
}

function addArticleToList(article, rowNo) {
    const row = document.createElement('tr');

    const rowNumberCell = document.createElement('th');
    rowNumberCell.textContent = `${rowNo}.`;
    row.appendChild(rowNumberCell);

    const titleCell = document.createElement('td');
    titleCell.textContent = article.title;
    row.appendChild(titleCell);

    const dopCell = document.createElement('td');
    const dopValue = new Date(article.dateOfPublication);
    dopCell.textContent = `${months[dopValue.getMonth()]} ${dopValue.getDate()}, ${dopValue.getFullYear()}`;
    row.appendChild(dopCell);

    const numOfPagesCell = document.createElement('td');
    numOfPagesCell.textContent = Number(article.numberOfPages);
    row.appendChild(numOfPagesCell);

    const categoryCell = document.createElement('td');
    categoryCell.textContent = categoryEnum[article.category] || "Invalid Category Code";
    row.appendChild(categoryCell);

    articlesTableBody.appendChild(row);
}

function loadArticleAssignmentForm() {
    fetch(`/api/ScientificArticles/not-by-scientist/${scientistId}`, {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            return checkResponseData(response);
        })
        .then(data => {
            loadFormData(data);
        })
        .catch(err => {
            console.log(`Error status ${err.status}: ${err.message}`);
            alert(`Something went wrong while loading article selection.. :-/
                Check the console for details.`);
        });
}

function checkResponseData(response){
    if (!response) {
        console.log("response is null");
        return [];
    } else if (response.status === 204) {
        console.log("No articles left to assign.");
        return [];
    } else if (!response.ok) {
        console.log("response NOT ok! >:(");
        return response.text().then(errorText => {
            throw new Error(`HTTP Error ${response.status}: ${errorText}`);
        });
    }
    return response.json();
}

function loadFormData(data) {
    let selectArticleElem = document.getElementById('articlesList');
    selectArticleElem.innerHTML = '';
    resetLeadResearcherCheckbox();
    if (data.length === 0) {
        let option = document.createElement('option');
        option.value = "";
        option.innerText = "No articles available";
        selectArticleElem.appendChild(option);
    } else {
        data.forEach(article => {
            let option = document.createElement("option");
            option.value = article.id;
            option.innerText = article.title;
            selectArticleElem.appendChild(option);
        });
    }
}

function resetLeadResearcherCheckbox() {
    const isLeadResCheckBbox = document.getElementById('isLeadResearcherCheckbox');
    isLeadResCheckBbox.checked = false;
}