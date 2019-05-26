import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CounterComponent } from '../counter/counter.component';

@NgModule({
  imports:      [ BrowserModule ],
  declarations: [ AppComponent,CounterComponent, ],
    bootstrap: [AppComponent]
})

@NgModule({imports: [
        RouterModule.forRoot([{ path: 'counter', component: CounterComponent }
        ])
    ]
})
export class AppModule { }
