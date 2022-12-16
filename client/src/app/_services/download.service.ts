import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class DownloadService {
  baseUrl = 'http://localhost:22142/api/';
  paginatedResult: PaginatedResult<any[]> = new PaginatedResult<any[]>();

constructor(private http: HttpClient) { }

  download(accessToken?: any, expirationTime?: any, page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if (accessToken !== null && typeof expirationTime !== 'undefined' && page !== null && itemsPerPage !== null) {
      params = params.append('accessToken', accessToken!.toString());
      params = params.append('expirationTime', expirationTime!.toString());
      params = params.append('pageNumber', page!.toString());
      params = params.append('pageSize', itemsPerPage!.toString());
    }

    return this.http.get(this.baseUrl + 'Download', {observe: 'response', params}).pipe(
      map((response: any) => {
        this.paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return this.paginatedResult;
      })
    );
  }

}
