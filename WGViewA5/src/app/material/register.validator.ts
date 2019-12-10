import { FormGroup } from '@angular/forms';

export class RegistrationValidator {
  static validate(registrationAccountGroup: FormGroup) {
    let newPassword = registrationAccountGroup.controls.newPassword.value;
    let confirmPassword = registrationAccountGroup.controls.confirmPassword.value;

    if (confirmPassword.length <= 0) {
      return null;
    }

    if (confirmPassword !== newPassword) {
      return {
        doesMatchPassword: true
      };
    }

    return null;

  }
}
