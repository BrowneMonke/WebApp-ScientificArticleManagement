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
    
    fetch('/api/ArticleScientistLinks', {
        method: 'POST', headers: {
            'Accept': 'application/json', 'Content-Type': 'application/json'
        }, body: JSON.stringify({
            "articleId": articleId, "scientistId": scientistId, "isLeadResearcher": isLeadResearcher
        })
    })
        .then(response => {
            if (!response.ok) {
                if (response.status === 204) {
                    console.log("No articles available to assign.");
                    return [];
                } else {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
            }
            console.log("Article assigned successfully!");
            init();
            return response.json();
        })
        .catch(error => {
            console.error("Error assigning article:", error);
        });
}

function init() {
    loadArticlesOfScientist();
    loadArticleAssignmentForm();
}


function loadArticlesOfScientist() {
    fetch(`/api/ScientificArticles/${scientistId}`, {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => showArticles(data))
        .catch(err => alert(`Something went wrong.. :-/
            ${err.message}`));
}

function showArticles(articles) {
    articlesTableBody.innerHTML = '';
    articles.forEach(art => addArticleToList(art));
}

function addArticleToList(article) {
    const row = document.createElement('tr');

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
    fetch(`/api/ScientificArticles/articles/not-by/${scientistId}`, {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => {
            let selectArticleElem = document.getElementById('articlesList');
            selectArticleElem.innerHTML = '';
            data.forEach(article => {
                let option = document.createElement("option");
                option.value = article.id;
                option.innerText = article.title;
                selectArticleElem.appendChild(option);
            })
        })
        .catch(err => {
            alert(`Something went wrong while loading article selection.. :-/
                ${err.message}`);
        });
}

/*
function saveArticleData(articles) {
    articles.forEach(article => {
        articlesInfo[article.id] = article.title; 
    });
}
*/


/*
function createAddArticleForm() {
    const addArticleFormElem = document.createElement('form');
    addArticleFormElem.classList.add('form');

    const articleSelectionDivElem = document.createElement('div');
    articleSelectionDivElem.classList.add('form-group');
    articleSelectionDivElem.classList.add('row');
    
    const selectArticleLabelElem = document.createElement('label');
    selectArticleLabelElem.textContent = "Select article";
    selectArticleLabelElem.setAttribute('asp-for', 'Category');
    selectArticleLabelElem.classList.add('col-2');
    
    const selectArticleElem = document.createElement('select');
    selectArticleElem.setAttribute('asp-for', 'Category');
    selectArticleElem.classList.add('col-6');
    
    articleSelectionDivElem.appendChild(selectArticleLabelElem);
    articleSelectionDivElem.appendChild(selectArticleElem);
    
    for (const articleId of Object.keys(articlesInfo)) {
        console.log(`${Number(articleId)} : ${articlesInfo[Number(articleId)]}`);
        const optionElem = document.createElement('option');
        optionElem.value = articleId;
        optionElem.textContent = articlesInfo[Number(articleId)];
        selectArticleElem.appendChild(optionElem);
    }
    
    const isLeadResearcherCheckboxElem = createCheckbox("Is Lead Researcher?", "isLeadResearcher", "leadResearcher", false);
    
    addArticleFormElem.appendChild(articleSelectionDivElem);
    addArticleFormElem.appendChild(isLeadResearcherCheckboxElem);
    
    return addArticleFormElem;
}

function createCheckbox(label, name, id, isChecked) {
    const container = document.createElement('div');
    container.classList.add('form-check'); // For Bootstrap styling
    container.classList.add('row');

    const labelElement = document.createElement('label');
    // labelElement.htmlFor = id; // Associate the label with the checkbox
    labelElement.classList.add('form-check-label'); // For Bootstrap styling
    labelElement.classList.add('col-1');
    labelElement.textContent = label;
    labelElement.setAttribute('asp-for', name); // Associate the label with the checkbox

    const checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    // checkbox.id = id;       // Important for label association and JavaScript access
    checkbox.classList.add('form-check-input'); // For Bootstrap styling 
    checkbox.classList.add('col-1');
    checkbox.checked = isChecked; // Set the initial checked state
    checkbox.setAttribute('asp-for', name)

    container.appendChild(labelElement);
    container.appendChild(checkbox);

    return container;
}*/
