package me.alistairmclean.cc01;

import android.os.AsyncTask;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.ByteArrayOutputStream;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Created by alist on 11/6/2017.
 */

public class TCPClient extends RCCarActivity {
    private String mHostIp = "";
    private int mHostPort = 2142;
    public TCPClient(String ip, int port) {
        mHostIp = ip;
        mHostPort = port;
    }

    class BackgroundTask extends AsyncTask<int[],Void,Void>
{
    private Socket sock;
    private PrintWriter writer;
    private ByteArrayOutputStream baos;
    @Override
    protected Void doInBackground(int[]... voids) {
        try {
            int[] data = voids[0];
            sock = new Socket(mHostIp, mHostPort);
            writer = new PrintWriter(sock.getOutputStream());
            writer.write(data.toString());
            writer.flush();
            writer.close();

        } catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

}
}
