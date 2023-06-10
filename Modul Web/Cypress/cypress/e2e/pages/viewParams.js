class ViewParams {
    elements = {
        formTemperature: () => cy.get('#temperature'),
        formLight: () => cy.get('#light'),
        formLight: () => cy.get('#light'),
        formOpenDoor: () => cy.get('input[type="radio"][value="true"]'),
        formCloseDoor: () => cy.get('input[type="radio"][value="false"]'),
        submitButton: () => cy.get('#submit')
    }

    completeFormWithOpenedDoor(temp, light) {
        this.elements.formTemperature().clear().type(temp)
        this.elements.formLight().invoke('val', light).trigger('change')
        this.elements.formOpenDoor().check()
    }

    submitButton() {
        this.elements.submitButton().click();
    }
}

export default ViewParams;