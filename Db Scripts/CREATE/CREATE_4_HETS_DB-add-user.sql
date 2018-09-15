-- trdbhetd -> dev account
-- trdbhett -> test account
-- trdbhetp -> prod account
create user trdbhett;
alter user trdbhett with encrypted password '<pwd>';
grant het_application_proxy to trdbhett;
