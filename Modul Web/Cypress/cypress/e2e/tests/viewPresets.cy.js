import Login from "../pages/login"

const randomNumber = Math.floor(Math.random() * 1000);

describe('Preset Creation', () => {
    beforeEach(() => {
        const login = new Login()

        cy.visit('https://localhost:7274/Authentication/Login')
        login.completeForm("admin@admin", "admin")
        login.submitButton()

        cy.visit('https://localhost:7274/User/ViewPresets')
    })

    it('Should create a new preset and verify its presence', () => {
        const preset = 'Test Preset';
        const presetName = `${preset}${randomNumber}`;
        const temperature = 25;
        const light = 75;


        cy.intercept({
            url: 'https://localhost:7274/User/CreatePreset',
            method: 'POST'
        }).as('createPreset')

        cy.intercept({
            url: 'https://localhost:7274/User/ViewPresets?PresetSuccess=True',
            method: 'GET'
        }).as('viewPresets')

        cy.get('input[name="Preset.Preset_Name"]').type(presetName);
        cy.get('input[name="Preset.Temperature"]').clear().type(temperature);
        cy.get('input[name="Preset.Light"]').invoke('val', light).trigger('change');

        cy.get('#createPreset').click()

        cy.get('@createPreset').its('response.statusCode').should('eq', 302);
        cy.get('@viewPresets').its('response.statusCode').should('eq', 200);

        cy.get('.alert-success').should('be.visible');

        cy.get('table')
            .should('contain', presetName)
            .and('contain', `${light}`)
            .and('contain', `${temperature}`);

        cy.get('input[readonly]').should('have.value', presetName);
    });

    it('Should select a preset and verify the current preset input', () => {
        const preset = 'Test Preset';
        const selectedPreset = `${preset}${randomNumber}`;

        cy.intercept({
            url: 'https://localhost:7274/User/SetPreset',
            method: 'POST'
        }).as('setPreset')

        cy.intercept({
            url: 'https://localhost:7274/User/ViewPresets?SetPresetSuccess=True',
            method: 'GET'
        }).as('viewPresets')

        cy.get('select[name="SelectedPreset"]').select(selectedPreset);

        cy.get('#applyPreset').click()

        cy.wait('@setPreset').its('response.statusCode').should('eq', 302);
        cy.get('@viewPresets').its('response.statusCode').should('eq', 200);

        cy.get('.alert-success').should('be.visible');
        cy.get('input[readonly]').should('have.value', selectedPreset);
    });

});
