

class WageSettingDto {
    constructor(from, to, wage) {
        this.from = from || 0;
        this.to = to || 0;
        this.wage = wage || 0;
    }
}


let casheBackWages = [];
let inPersonWages = [];


function renderInPersonWageTable() {
    const tableBody = document.getElementById("inPersonWagesTableBody");
    tableBody.innerHTML = inPersonWages.map((element, index) => `
        <tr>
            <td>${element.from}</td>
            <td>${element.to}</td>
            <td>${element.wage}</td>
            <td>
                <button type="button" class="btn btn-danger btn-sm" onclick="deleteInPersonWage(${index})">
                    <i class="fa fa-trash me-1"></i> حذف
                </button>
            </td>
        </tr>
    `).join("");
}



function renderCashBackWageTable() {
    const tableBody = document.getElementById("cashBackWagesTableBody");
    tableBody.innerHTML = casheBackWages.map((element, index) => `
        <tr>
            <td>${element.from}</td>
            <td>${element.to}</td>
            <td>${element.wage}</td>
            <td>
                <button type="button" class="btn btn-danger btn-sm" onclick="deleteCashBackWage(${index})">
                    <i class="fa fa-trash me-1"></i> حذف
                </button>
            </td>
        </tr>
    `).join("");
}



function deleteInPersonWage(itemId) {
    console.log('deleting Item ' + itemId);
    inPersonWages = inPersonWages.filter((data, idx) => idx !== itemId);
    console.log(inPersonWages);
    renderInPersonWageTable();
}

function deleteCashBackWage(itemId) {
    console.log('deleting Item ' + itemId);
    casheBackWages = casheBackWages.filter((data, idx) => idx !== itemId);
    console.log(casheBackWages);
    renderCashBackWageTable();
}


function AddCashBackWage() {
    const fromInput = document.querySelector("#cashback-from");
    const toInput = document.querySelector("#cashback-to");
    const wageInput = document.querySelector("#cashback-wage");

    const from = parseFloat(fromInput.value);
    const to = parseFloat(toInput.value);
    const wage = parseFloat(wageInput.value);

    // Validate Inputs
    if (isNaN(from) || isNaN(to) || isNaN(wage) || from <= 0 || to <= 0 || wage <= 0) {
        alert("لطفا همه فیلدها را با مقادیر معتبر پر کنید.");
        return;
    }

    // Add to Global Array
    casheBackWages.push(new WageSettingDto(from, to, wage));

    // Clear Inputs
    fromInput.value = "";
    toInput.value = "";
    wageInput.value = "";

    // Re-render Table
    renderCashBackWageTable();
}


function AddInPersonWage() {
    const fromInput = document.querySelector("#inperson-from");
    const toInput = document.querySelector("#inperson-to");
    const wageInput = document.querySelector("#inperson-wage");

    const from = parseFloat(fromInput.value);
    const to = parseFloat(toInput.value);
    const wage = parseFloat(wageInput.value);

    // Validate Inputs
    if (isNaN(from) || isNaN(to) || isNaN(wage) || from <= 0 || to <= 0 || wage <= 0) {
        alert("لطفا همه فیلدها را با مقادیر معتبر پر کنید.");
        return;
    }

    // Add to Global Array
    inPersonWages.push(new WageSettingDto(from, to, wage));

    // Clear Inputs
    fromInput.value = "";
    toInput.value = "";
    wageInput.value = "";

    // Re-render Table
    renderInPersonWageTable();
}


function setCasheBackWage(formData) {
    casheBackWages.forEach((item, index) => {
        formData.set(`CashBackSettingWages[${index}].From`, item.from);
        formData.set(`CashBackSettingWages[${index}].To`, item.to);
        formData.set(`CashBackSettingWages[${index}].Wage`, item.wage);
    });
}

function setInPersonWage(formData) {
    inPersonWages.forEach((item, index) => {
        formData.set(`InPersonSettingWages[${index}].From`, item.from);
        formData.set(`InPersonSettingWages[${index}].To`, item.to);
        formData.set(`InPersonSettingWages[${index}].Wage`, item.wage );
    });
}



