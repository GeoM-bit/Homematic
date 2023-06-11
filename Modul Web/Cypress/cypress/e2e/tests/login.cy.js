import Login from "../pages/login"

context('Homematic Automation Testing', () => {
    const login = new Login();
    beforeEach(() => {
        cy.visit('https://localhost:7274/Authentication/Login')
    })

    it('Verify unregistered email', () => {

        login.completeForm(
            'ana@dunea',
            'justApassword'
        )

        login.submitButton()

        cy.get('.alert').should('contain.text', 'Invalid credentials!')
    })

    it('Verify incorrect password', () => {

        login.completeForm(
            'adin@admin',
            'notTheCorrectPassword'
        )

        login.submitButton()

        cy.get('.alert').should('contain.text', 'Invalid credentials!')
    })

    it('Verify user with invalid IMEI', () => {

        login.completeForm(
            'ana.dunea@gmail.com',
            'aha'
        )

        login.submitButton()

        cy.get('.alert').should('contain.text', 'Phone device not verified! Please login in the mobile app first.')
    })

    it('Verify login request', () => {

        login.completeForm(
            'admin@admin',
            'admin'
        )
        cy.intercept({
            url: 'https://localhost:7274/Authentication/LoginUser',
            method: 'POST'
        }).as('loginRequest')

        cy.intercept({
            url: 'https://localhost:7274/User/ViewParameters',
            method: 'GET'
        }).as('viewParamsRequest')

        login.submitButton()

        cy.get('@loginRequest').its('response.statusCode').should('eq', 302)
        cy.get('@viewParamsRequest').its('response.statusCode').should('eq', 200)

    })

})