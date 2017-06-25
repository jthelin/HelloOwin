FROM mono:onbuild
EXPOSE 12345
CMD [ "mono", "./HelloOwinServer.exe" ]
