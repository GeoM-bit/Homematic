import Login from "../pages/login"
import Register from "../pages/register"

context('Homematic Automation Testing', () => {
    beforeEach(() => {
        const login = new Login()
        const register = new Register()

        cy.visit('https://localhost:7274/Authentication/Login')
        login.completeForm("admin@admin", "admin")
        login.submitButton()

        cy.visit('https://localhost:7274/Authentication/Register')
        register.completeForm("George", "Popescu", "george.popescu@gmail.com", "myPassword", "6001112360022")
        register.submitButton()

        cy.visit('https://localhost:7274/Admin/ViewUsers')
    })

    it('Verify successfully deleted user', () => {

        const emailToDelete = 'george.popescu@gmail.com'

        cy.get('#dataTable')
            .contains('td', emailToDelete)
            .parent('tr')
            .find('a[data-testid="deleteButton"]') 
            .click();

        cy.get('#deleteModal')  
            .find('#btnDelete')  
            .click();

        cy.get('.alert').should('contain.text', 'The user was successfully deleted!')
        cy.get('table').should('not.contain', emailToDelete); 
    })
})