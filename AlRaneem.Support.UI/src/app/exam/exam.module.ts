import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExamRoutes } from './exam-routing.module';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(ExamRoutes),
  ]
})
export class ExamModule { }
