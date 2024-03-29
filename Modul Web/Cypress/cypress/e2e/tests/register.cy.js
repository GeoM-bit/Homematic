import Register from "../pages/register"
import Login from "../pages/login"

context('Homematic Automation Testing', () => {
    const register = new Register();
    const login = new Login();
    beforeEach(() => {
        cy.visit('https://localhost:7274/Authentication/Login')
        login.completeForm("admin@admin", "admin")
        login.submitButton()
        cy.visit('https://localhost:7274/Authentication/Register')
    })

    it("Verify register users form validation", () => {
        register.completeFormWithoutPasswordAndCNP("Maria", "Costea", "maria.costa@gmail.com")

        register.submitButton()

        cy.get('#passwordError').should('contain.text', 'Password is required!')
        cy.get('#CNPError').should('contain.text', 'CNP is required!')
    })

    it("Verify success message after registering user", () => {
        register.completeForm("Dan", "Marta", "dan.marta@gmail.com","myPassword","6000812360022")

        register.submitButton()

        cy.get('.alert').should('contain.text', 'The account was successfully created!')
    })

    it("Verify failure message after trying to register existing user", () => {
        register.completeForm("Dan", "Marta", "dan.marta@gmail.com", "myPassword", "6000812360022")

        register.submitButton()

        cy.get('.alert').should('contain.text', 'The account could not be created!')
    })
})