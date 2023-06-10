class Login {
    elements = {
        formEmail: () => cy.get('input[data-testid="LoginEmail"]'),
        formPassword: () => cy.get('#LoginPassword'),
        submitButton: () => cy.get('#submitLogin')
    }

    completeForm(email, password) {
        this.elements.formEmail().type(email)
        this.elements.formPassword().type(password)
    }

    submitButton() {
        this.elements.submitButton().click();
    }
}

export default Login;