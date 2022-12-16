import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../_models/pagination';
import { DownloadService } from '../_services/download.service';

@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.css']
})
export class DownloadComponent implements OnInit {
  resourcePaths: any;
  resourcePathsList: any;
  accessToken: any;
  expirationTime: any;
  pagination?: Pagination;
  pageNumber = 1;
  pageSize = 25;

  constructor(private downloadService: DownloadService, private toastr: ToastrService) { }

  ngOnInit() {
    this.download();
  }

  download(){
    let token = sessionStorage.getItem('accessToken');
    if (token !== null && typeof token !== 'undefined') {
      this.accessToken = token.substring(1, token.length-1);
    }
    
    let exp = sessionStorage.getItem('expirationTime')?.slice(1,20).toString();
    if (exp !== null && typeof exp !== 'undefined') {

      this.expirationTime = new Date(exp);
      this.expirationTime.setHours(this.expirationTime.getHours() + 1);
      this.expirationTime = this.expirationTime.toUTCString();
    }

    this.toastr.success("List is downloading.");

    this.downloadService.download(this.accessToken, this.expirationTime, this.pageNumber, this.pageSize).subscribe(response => {
      this.resourcePaths = response.result;
      this.pagination = response.pagination;
      this.resourcePathsList = this.resourcePaths.resourcePathsList;
      this.toastr.success("List downloaded successfully.");

      sessionStorage.setItem('accessToken', JSON.stringify(this.resourcePaths.authTokenData.accessToken));
      sessionStorage.setItem('expirationTime', JSON.stringify(this.resourcePaths.authTokenData.expirationTime));
    }, error => {
      this.toastr.error(error.error);
    });
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.download();
  }
}
