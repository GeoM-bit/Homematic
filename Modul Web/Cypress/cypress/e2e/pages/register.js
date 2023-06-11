class Register {
    elements = {
        formFirstName: () => cy.get('#firstName'),
        formLastName: () => cy.get('#lastName'),
        formEmail: () => cy.get('#email'),
        formPassword: () => cy.get('#password'),
        formCNP: () => cy.get('#CNP'),
        submitButton: () => cy.get('#submit')
    }

    completeFormWithoutPasswordAndCNP(firstName, lastName, email) {
        this.elements.formFirstName().type(firstName)
        this.elements.formLastName().type(lastName)
        this.elements.formEmail().type(email)
    }
    completeForm(firstName, lastName, email, password,CNP) {
        this.elements.formFirstName().type(firstName)
        this.elements.formLastName().type(lastName)
        this.elements.formEmail().type(email)
        this.elements.formPassword().type(password)
        this.elements.formCNP().type(CNP)
    }

    submitButton() {
        this.elements.submitButton().click();
    }
}

export default Register;