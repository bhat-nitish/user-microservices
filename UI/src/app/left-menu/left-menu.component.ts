import { Component, OnInit } from '@angular/core';
import { SidenavService } from '../sidenav.service'
import { onSideNavChange, animateText } from '../animations/animation'


interface Page {
  link: string;
  name: string;
  icon: string;
}

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss'],
  animations: [onSideNavChange, animateText]
})

export class LeftMenuComponent implements OnInit {

  public sideNavState: boolean = false;
  public linkText: boolean = false;

  public pages: Page[] = [
    { name: 'View Users', link: '/users/list', icon: 'account_circle' },
    { name: 'Add User', link: '/users', icon: 'person_add' },
    { name: 'Manage Users', link: '/admin/users/list', icon: 'admin_panel_settings' }
  ]

  constructor(private _sidenavService: SidenavService) { }

  ngOnInit() {
  }

  onSinenavToggle() {
    this.sideNavState = !this.sideNavState

    setTimeout(() => {
      this.linkText = this.sideNavState;
    }, 200)
    this._sidenavService.sideNavState$.next(this.sideNavState)
  }


}
