import ViewParams from "../pages/viewParams"
import Login from "../pages/login"

context('Homematic Automation Testing', () => {
    const viewParams = new ViewParams();
    const login = new Login();
    beforeEach(() => {
        cy.visit('https://localhost:7274/Authentication/Login')
        login.completeForm("admin@admin", "admin")
        login.submitButton()
    })

    it("Verify parameters change after submit", () => {
        viewParams.completeFormWithOpenedDoor('20', '50')

        cy.intercept({
            url: 'https://localhost:7274/User/ModifyParameters',
            method: 'POST'
        }).as('modifyParamsRequest')

        cy.intercept({
            url: 'https://localhost:7274/User/ViewParameters',
            method: 'GET'
        }).as('viewParamsRequest')

        viewParams.submitButton()

        cy.get('@modifyParamsRequest').its('response.statusCode').should('eq', 302)
        cy.get('@viewParamsRequest').its('response.statusCode').should('eq', 200)

        cy.get('#temperatureCell').should('contain.text', '20')
        cy.get('#lightCell').should('contain.text', '50')
        cy.get('#doorCell').should('have.text', 'opened')
    })
})