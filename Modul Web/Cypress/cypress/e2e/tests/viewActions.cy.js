import Login from "../pages/login"

context('Homematic Automation Testing', () => {
    const login = new Login();
    beforeEach(() => {
        cy.visit('https://localhost:7274/Authentication/Login')
        login.completeForm("admin@admin", "admin")
        login.submitButton()
    })

    it('Verify get actions request', () => {
        cy.intercept({
            url: 'https://localhost:7274/User/ViewActions',
            method: 'GET'
        }).as('viewActionsRequest')

        cy.visit('https://localhost:7274/User/ViewActions')

        cy.wait('@viewActionsRequest').its('response.statusCode').should('eq', 200);
    })
})