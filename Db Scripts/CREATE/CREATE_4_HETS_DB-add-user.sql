-- trdbhetd -> dev account
-- trdbhett -> test account
-- trdbhetp -> prod account
create user trdbhetd;
alter user trdbhetd with encrypted password '<pwd>';
grant het_application_proxy to trdbhetd;
