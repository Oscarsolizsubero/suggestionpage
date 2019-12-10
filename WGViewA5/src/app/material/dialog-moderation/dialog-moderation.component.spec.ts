import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogModerationComponent } from './dialog-moderation.component';

describe('DialogModerationComponent', () => {
  let component: DialogModerationComponent;
  let fixture: ComponentFixture<DialogModerationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogModerationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogModerationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
